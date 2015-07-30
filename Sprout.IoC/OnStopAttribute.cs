using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An attribute that can be applied to Component methods. This will get invoked after the Context has been stopped.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OnStopAttribute : Attribute
    {
    }
}
