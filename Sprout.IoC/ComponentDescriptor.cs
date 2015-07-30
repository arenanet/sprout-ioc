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
