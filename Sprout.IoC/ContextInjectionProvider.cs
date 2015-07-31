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
    /// An injection provider that provides the Context to classes.
    /// </summary>
    internal class ContextInjectionProvider : IInjectionProvider
    {
        /// <summary>
        /// Resolves Context injections.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="componentDescriptor"></param>
        /// <param name="injectDescriptor"></param>
        /// <returns></returns>
        public object ResolveInject(Context context, ComponentDescriptor componentDescriptor, InjectDescriptor injectDescriptor)
        {
            if (injectDescriptor.Type.Equals(context.GetType().AssemblyQualifiedName))
            {
                return context;
            }

            return null;
        }
    }
}
