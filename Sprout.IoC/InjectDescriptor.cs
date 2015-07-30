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
