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
    /// Internal descriptor for a component. This is primarily used to cache reflection lookups.
    /// </summary>
    public class ComponentDescriptor
    {
        public Type Type
        {
            private set;
            get;
        }

        public ComponentAttribute Attributes
        {
            private set;
            get;
        }

        public List<InjectDescriptor> Injections
        {
            private set;
            get;
        }

        public ComponentDescriptor(Type type, ComponentAttribute componentAttribute)
        {
            this.Type = type;
            this.Attributes = componentAttribute;
            this.Injections = new List<InjectDescriptor>();

            foreach (MemberInfo member in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (member is FieldInfo || member is PropertyInfo)
                {
                    InjectAttribute injectAttribute = null;

                    if ((injectAttribute = member.GetCustomAttribute<InjectAttribute>()) != null)
                    {
                        Injections.Add(new InjectDescriptor(member, injectAttribute));
                    }
                }
            }
        }
    }
}
