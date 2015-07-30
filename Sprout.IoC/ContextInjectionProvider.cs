using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An injection provider that provides the Context to classes.
    /// </summary>
    internal class ContextInjectionProvider : IInjectionProvider
    {
        /// <summary>
        /// Resolves Context injections.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="componentDescriptor"></param>
        /// <param name="injectDescriptor"></param>
        /// <returns></returns>
        public object ResolveInject(Context context, ComponentDescriptor componentDescriptor, InjectDescriptor injectDescriptor)
        {
            if (injectDescriptor.Type.Equals(context.GetType().AssemblyQualifiedName))
            {
                return context;
            }

            return null;
        }
    }
}
