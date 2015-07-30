using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// A processor that gets invoked during the Component lifecycle.
    /// </summary>
    public interface IComponentLifecycleProcessor
    {
        /// <summary>
        /// Invoked when the component is started (meaning its injections have been resolved).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="descriptor"></param>
        void OnStart(Context context, object component, ComponentDescriptor descriptor);

        /// <summary>
        /// Invoked when the context shuts down.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="descriptor"></param>
        void OnStop(Context context, object component, ComponentDescriptor descriptor);
    }
}
