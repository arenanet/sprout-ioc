/*
 * Copyright 2015 ArenaNet, LLC.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this 
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * 	 http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under 
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF 
 * ANY KIND, either express or implied. See the License for the specific language governing 
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// The central IoC context. This context is self contained and you can have multiple instances if needed.
    /// </summary>
    public sealed class Context : IDisposable
    {
        public ContextState State
        {
            private set;
            get;
        }

        private readonly object _contextMutex = new object();

        internal HashSet<IInjectionProvider> injectionProviders = 
            new HashSet<IInjectionProvider>(new TypeUniqueEqualityComparer<IInjectionProvider>());
        internal HashSet<IComponentLifecycleProcessor> componentLifecycleProcessors = 
            new HashSet<IComponentLifecycleProcessor>(new TypeUniqueEqualityComparer<IComponentLifecycleProcessor>());

        internal Dictionary<string, object> singletonsByName = new Dictionary<string, object>();
        internal Dictionary<string, HashSet<object>> singletonsByType = new Dictionary<string, HashSet<object>>();

        internal Dictionary<string, ComponentDescriptor> descriptorTypeCache = new Dictionary<string, ComponentDescriptor>();
        internal Dictionary<string, ComponentDescriptor> descriptorNameCache = new Dictionary<string, ComponentDescriptor>();

        /// <summary>
        /// Stops the Context.
        /// </summary>
        ~Context()
        {
            Dispose();
        }

        /// <summary>
        /// Starts the context. Note: You can only call this method once per Context.
        /// </summary>
        public void Start()
        {
            AssertState(ContextState.Created);

            lock (_contextMutex)
            {
                AssertState(ContextState.Created);

                injectionProviders.Add(new ContextInjectionProvider());
                injectionProviders.Add(new ComponentInjectionProvider());

                componentLifecycleProcessors.Add(new NotifyingComponentLifecycleProcessor());

                State = ContextState.Starting;

                foreach (ComponentDescriptor descriptor in descriptorTypeCache.Values)
                {
                    GetInstance(descriptor);
                }

                State = ContextState.Started;
            }
        }

        /// <summary>
        /// Stops the context. Note: You can only call this method once per Context.
        /// </summary>
        public void Stop()
        {
            AssertState(ContextState.Started);

            lock (_contextMutex)
            {
                AssertState(ContextState.Started);

                State = ContextState.Stopping;

                foreach(KeyValuePair<string, object> kvp in singletonsByName)
                {
                    ComponentDescriptor descriptor = null;

                    if (descriptorNameCache.TryGetValue(kvp.Key, out descriptor))
                    {
                        foreach (IComponentLifecycleProcessor componentLifecycleProcessor in componentLifecycleProcessors)
                        {
                            componentLifecycleProcessor.OnStop(this, kvp.Value, descriptor);
                        }
                    }
                }

                singletonsByName.Clear();
                singletonsByType.Clear();

                descriptorTypeCache.Clear();
                descriptorNameCache.Clear();

                State = ContextState.Stopping;
            }
        }

        /// <summary>
        /// Adds an injection provider.
        /// </summary>
        /// <param name="provider"></param>
        public void AddInjectionProvider(IInjectionProvider provider)
        {
            AssertState(ContextState.Created);

            lock (_contextMutex)
            {
                AssertState(ContextState.Created);

                injectionProviders.Add(provider);
            }
        }

        /// <summary>
        /// Removes an injection provider.
        /// </summary>
        /// <param name="provider"></param>
        public void RemoveInjectionProvider(IInjectionProvider provider)
        {
            AssertState(ContextState.Created);

            lock (_contextMutex)
            {
                AssertState(ContextState.Created);

                injectionProviders.Remove(provider);
            }
        }

        /// <summary>
        /// Adds a component lifecycle processor.
        /// </summary>
        /// <param name="provider"></param>
        public void AddComponentLifecycleProcessor(IComponentLifecycleProcessor processor)
        {
            AssertState(ContextState.Created);

            lock (_contextMutex)
            {
                AssertState(ContextState.Created);

                componentLifecycleProcessors.Add(processor);
            }
        }

        /// <summary>
        /// Adds a component lifecycle processor.
        /// </summary>
        /// <param name="provider"></param>
        public void RemoveComponentLifecycleProcessor(IComponentLifecycleProcessor processor)
        {
            AssertState(ContextState.Created);

            lock (_contextMutex)
            {
                AssertState(ContextState.Created);

                componentLifecycleProcessors.Remove(processor);
            }
        }

        /// <summary>
        /// Registers a component by type with this context. Note: You can only call this on a Context that hasn't started.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public Context Register<T>()
        {
            AssertState(ContextState.Created);

            return Register(typeof(T));
        }

        /// <summary>
        /// Registers a component by type with this context. Note: You can only call this on a Context that hasn't started.
        /// </summary>
        /// <param name="type"></param>
        public Context Register(Type type)
        {
            AssertState(ContextState.Created);

            if (type == null)
            {
                throw new ArgumentNullException("Type cannot be null.");
            }

            ComponentAttribute componentAttribute = type.GetCustomAttribute<ComponentAttribute>();

            if (componentAttribute == null)
            {
                throw new ArgumentException("Type: '" + type + "' is not marked as a Component.");
            }

            RegisterComponent(type, componentAttribute);

            return this;
        }

        /// <summary>
        /// Scans the namespace of the given type and all of it's child namespaces for components. Note: You can only call this on a Context that hasn't started.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scanAssembly"></param>
        public Context Scan<T>(Assembly scanAssembly = null)
        {
            AssertState(ContextState.Created);

            return Scan(typeof(T), scanAssembly);
        }

        /// <summary>
        /// Scans the namespace of the given type and all of it's child namespaces for components. Note: You can only call this on a Context that hasn't started.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="scanAssembly"></param>
        public Context Scan(Type type, Assembly scanAssembly = null)
        {
            AssertState(ContextState.Created);

            if (type == null)
            {
                throw new ArgumentNullException("Type cannot be null.");
            }

            return Scan(type.Namespace, scanAssembly);
        }

        /// <summary>
        /// Scans the given namespace and all of it's child namespaces for components. Note: You can only call this on a Context that hasn't started.
        /// </summary>
        /// <param name="scanNamespace"></param>
        /// <param name="scanAssembly"></param>
        public Context Scan(string scanNamespace, Assembly scanAssembly = null)
        {
            AssertState(ContextState.Created);

            if (scanAssembly != null)
            {
                ScanAssembly(scanNamespace, scanAssembly);
            }
            else
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        ComponentAttribute componentAttribute = null;

                        if ((scanNamespace == null || string.IsNullOrEmpty(scanNamespace) || (type.Namespace != null && (type.Namespace.Equals(scanNamespace) || type.Namespace.StartsWith(scanNamespace + ".")))) &&
                            (componentAttribute = type.GetCustomAttribute<ComponentAttribute>()) != null)
                        {
                            RegisterComponent(type, componentAttribute);
                        }
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Scans a namespaces for Components.
        /// </summary>
        /// <param name="scanNamespace"></param>
        /// <param name="assembly"></param>
        private void ScanAssembly(string scanNamespace, Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                ComponentAttribute componentAttribute = null;

                if ((scanNamespace == null || string.IsNullOrEmpty(scanNamespace) || (type.Namespace != null && (type.Namespace.Equals(scanNamespace) || type.Namespace.StartsWith(scanNamespace + ".")))) &&
                    (componentAttribute = type.GetCustomAttribute<ComponentAttribute>()) != null)
                {
                    RegisterComponent(type, componentAttribute);
                }
            }
        }

        /// <summary>
        /// Gets a component by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetComponent(string name)
        {
            AssertState(ContextState.Started);

            object response = null;

            if (singletonsByName.TryGetValue(name, out response))
            {
                return response;
            }

            return null;
        }

        /// <summary>
        /// Gets a component by name and type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetComponent<T>(string name) where T : class
        {
            AssertState(ContextState.Started);
            return (T)GetComponent(name, typeof(T));
        }

        /// <summary>
        /// Gets a component by name and type.
        /// </summary>
        /// <param name="nam"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetComponent(string name, Type type)
        {
            AssertState(ContextState.Started);

            object response = null;

            if (singletonsByName.TryGetValue(name, out response))
            {
                return type.AssemblyQualifiedName.Equals(response.GetType().AssemblyQualifiedName) ? response : null;
            }

            return null;
        }

        /// <summary>
        /// Gets a component by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : class
        {
            AssertState(ContextState.Started);

            HashSet<object> components = null;

            if (singletonsByType.TryGetValue(typeof(T).AssemblyQualifiedName, out components))
            {
                if (components == null || components.Count < 1)
                {
                    return null;
                }

                object response = components.First();

                return (T)response;
            }

            return null;
        }

        /// <summary>
        /// Gets a list of components by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public HashSet<T> GetComponents<T>()
        {
            AssertState(ContextState.Started);

            HashSet<T> response = new HashSet<T>();

            HashSet<object> components = GetComponents(typeof(T));

            foreach (object component in components)
            {
                response.Add((T)component);
            }

            return response;
        }

        /// <summary>
        /// Gets a list of componnents by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public HashSet<object> GetComponents(Type type)
        {
            AssertState(ContextState.Started);

            HashSet<object> response = new HashSet<object>();

            HashSet<object> components = null;

            if (singletonsByType.TryGetValue(type.AssemblyQualifiedName, out components))
            {
                response.UnionWith(components);
            }

            return response;
        }

        /// <summary>
        /// Gets all the components in this context.
        /// </summary>
        /// <returns></returns>
        public HashSet<object> GetComponents()
        {
            HashSet<object> response = new HashSet<object>();

            response.UnionWith(singletonsByName.Values);

            return response;
        }

        /// <summary>
        /// Stops the context.
        /// </summary>
        public void Dispose()
        {
            if (State == ContextState.Started)
            {
                lock (_contextMutex)
                {
                    if (State == ContextState.Started)
                    {
                        Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Creates an instance of a component and caches if it is of Singletone scope.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        internal object GetInstance(ComponentDescriptor component)
        {
            object response = null;

            if (!singletonsByName.TryGetValue(component.Attributes.Name, out response))
            {
                response = Activator.CreateInstance(component.Type);

                if (component.Attributes.Scope == ComponentScope.Context)
                {
                    singletonsByName[component.Attributes.Name] = response;

                    Type currentType = component.Type;

                    while (currentType != null)
                    {
                        HashSet<object> singletonTypes = null;

                        if (!singletonsByType.TryGetValue(currentType.AssemblyQualifiedName, out singletonTypes))
                        {
                            singletonTypes = new HashSet<object>();
                            singletonsByType[currentType.AssemblyQualifiedName] = singletonTypes;
                        }

                        singletonTypes.Add(response);

                        currentType = currentType.BaseType;
                    }
                }

                foreach (InjectDescriptor inject in component.Injections)
                {
                    object injectionValue = null;

                    foreach (IInjectionProvider injectionProvider in injectionProviders)
                    {
                        injectionValue = injectionProvider.ResolveInject(this, component, inject);

                        if (injectionValue != null)
                        {
                            break;
                        }
                    }

                    if (injectionValue == null)
                    {
                        throw new ComponentNotFoundException(inject.Attributes.Type, inject.Attributes.Name);
                    }

                    if (inject.Member is FieldInfo)
                    {
                        ((FieldInfo)inject.Member).SetValue(response, injectionValue);
                    }
                    else if (inject.Member is PropertyInfo)
                    {
                        ((PropertyInfo)inject.Member).SetValue(response, injectionValue);
                    }
                }

                foreach (IComponentLifecycleProcessor componentLifecycleProcessor in componentLifecycleProcessors)
                {
                    componentLifecycleProcessor.OnStart(this, response, component);
                }
            }

            return response;
        }

        /// <summary>
        /// Registers a components of the given type and attributes.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="componentAttribute"></param>
        private void RegisterComponent(Type type, ComponentAttribute componentAttribute)
        {
            lock (_contextMutex)
            {
                AssertState(ContextState.Created);

                if (componentAttribute.Name == null)
                {
                    componentAttribute.Name = type.Name;
                }

                ComponentDescriptor existingDescriptor = null;

                if (descriptorNameCache.TryGetValue(componentAttribute.Name, out existingDescriptor))
                {
                    ComponentDescriptor descriptor = descriptorNameCache[componentAttribute.Name];

                    if (!existingDescriptor.Type.AssemblyQualifiedName.Equals(type.AssemblyQualifiedName))
                    {
                        throw new ComponentNameException("Component name '" + componentAttribute.Name + "' is already used by '" + descriptor.Type + "'");
                    }
                    else
                    {
                        return;
                    }
                }

                string typeName = type.AssemblyQualifiedName;

                if (descriptorTypeCache.ContainsKey(typeName))
                {
                    return;
                }

                ComponentDescriptor componentDescriptor = new ComponentDescriptor(type, componentAttribute);

                descriptorTypeCache[typeName] = componentDescriptor;
                descriptorNameCache[componentAttribute.Name] = componentDescriptor;
            }
        }

        /// <summary>
        /// Checks the current state of the context.
        /// </summary>
        /// <param name="expectedState"></param>
        private void AssertState(ContextState expectedState)
        {
            if (State != expectedState)
            {
                throw new InvalidContextStateException(expectedState, State);
            }
        }
    }
}