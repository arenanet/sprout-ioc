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
using System.Threading.Tasks;
using System.Reflection;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// A component lifecycle processor that notifies the Components of the lifecycle.
    /// </summary>
    internal class NotifyingComponentLifecycleProcessor : IComponentLifecycleProcessor
    {
        /// <summary>
        /// Notifies the given component when its Injections are complete.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="descriptor"></param>
        public void OnStart(Context context, object component, ComponentDescriptor descriptor)
        {
            foreach (MemberInfo member in descriptor.Type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (member is MethodInfo)
                {
                    if (member.GetCustomAttribute<OnStartAttribute>() != null)
                    {
                        ((MethodInfo)member).Invoke(component, new object[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Notifies the given component when the Context stops.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="descriptor"></param>
        public void OnStop(Context context, object component, ComponentDescriptor descriptor)
        {
            foreach (MemberInfo member in descriptor.Type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (member is MethodInfo)
                {
                    if (member.GetCustomAttribute<OnStopAttribute>() != null)
                    {
                        ((MethodInfo)member).Invoke(component, new object[0]);
                    }
                }
            }
        }
    }
}
