using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// The Component attribute that has to be applied to every component type in order for the Context to pick it up.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        public string Name
        {
            set;
            get;
        }

        public ComponentScope Scope
        {
            set;
            get;
        }

        public ComponentAttribute()
        {
            this.Scope = ComponentScope.Context;
        }
    }
}
