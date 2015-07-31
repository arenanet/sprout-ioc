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
    /// Internal descriptor for an injection.
    /// </summary>
    public class InjectDescriptor
    {
        public MemberInfo Member
        {
            private set;
            get;
        }

        public Type Type
        {
            get
            {
                if (Member is PropertyInfo)
                {
                    return ((PropertyInfo)Member).PropertyType;
                }
                else if (Member is FieldInfo)
                {
                    return ((FieldInfo)Member).FieldType;
                }

                return null;
            }
        }

        public InjectAttribute Attributes
        {
            private set;
            get;
        }

        public InjectDescriptor(MemberInfo member, InjectAttribute injectAttribute)
        {
            this.Member = member;
            this.Attributes = injectAttribute;
        }
    }
}
