using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An interface for providing injections.
    /// </summary>
    public interface IInjectionProvider
    {
        object ResolveInject(Context context, ComponentDescriptor componentDescriptor, InjectDescriptor injectDescriptor);
    }
}
