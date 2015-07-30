using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An exception that is thrown when a component name is invalid.
    /// </summary>
    public class ComponentNameException : ContextException
    {
        public ComponentNameException(string message) : base(message)
        {

        }
    }
}
