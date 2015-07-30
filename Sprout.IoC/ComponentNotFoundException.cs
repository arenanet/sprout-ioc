using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An exception that is thrown when the component is not found but required in the Context.
    /// </summary>
    public class ComponentNotFoundException : ContextException
    {
        public ComponentNotFoundException(Type componentType, string name) 
            : base("Component [type='" + (componentType == null ? "Any" : componentType.FullName) + "', name='" + name + "'] not found.")
        {

        }
    }
}
