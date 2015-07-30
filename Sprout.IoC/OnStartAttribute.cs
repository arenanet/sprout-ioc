using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An attribute that can be applied to Component methods. This will get invoked after the component has been initialized (all of its dependencies have been injected.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OnStartAttribute : Attribute
    {
    }
}
