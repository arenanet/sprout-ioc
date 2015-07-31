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

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// Provides component injection.
    /// </summary>
    internal class ComponentInjectionProvider : IInjectionProvider
    {
        /// <summary>
        /// Resolves an injection to a Component.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="componentDescriptor"></param>
        /// <param name="injectDescriptor"></param>
        /// <returns></returns>
        public object ResolveInject(Context context, ComponentDescriptor componentDescriptor, InjectDescriptor injectDescriptor)
        {
            ComponentDescriptor injectComponent = null;

            if (injectDescriptor.Attributes.Name != null)
            {
                context.descriptorNameCache.TryGetValue(injectDescriptor.Attributes.Name, out injectComponent);
            }

            if (injectDescriptor.Attributes.Type != null)
            {
                if (injectComponent == null)
                {
                    context.descriptorTypeCache.TryGetValue(injectDescriptor.Attributes.Type.AssemblyQualifiedName, out injectComponent);
                }
                else
                {
                    if (injectComponent.Type != injectDescriptor.Attributes.Type)
                    {
                        injectComponent = null;
                    }
                }
            }
            else
            {
                context.descriptorTypeCache.TryGetValue(injectDescriptor.Type.AssemblyQualifiedName, out injectComponent);
            }

            return injectComponent == null ? null : context.GetInstance(injectComponent);
        }
    }
}
