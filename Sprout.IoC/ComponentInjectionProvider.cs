using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// Provides component injection.
    /// </summary>
    internal class ComponentInjectionProvider : IInjectionProvider
    {
        /// <summary>
        /// Resolves an injection to a Component.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="componentDescriptor"></param>
        /// <param name="injectDescriptor"></param>
        /// <returns></returns>
        public object ResolveInject(Context context, ComponentDescriptor componentDescriptor, InjectDescriptor injectDescriptor)
        {
            ComponentDescriptor injectComponent = null;

            if (injectDescriptor.Attributes.Name != null)
            {
                context.descriptorNameCache.TryGetValue(injectDescriptor.Attributes.Name, out injectComponent);
            }

            if (injectDescriptor.Attributes.Type != null)
            {
                if (injectComponent == null)
                {
                    context.descriptorTypeCache.TryGetValue(injectDescriptor.Attributes.Type.AssemblyQualifiedName, out injectComponent);
                }
                else
                {
                    if (injectComponent.Type != injectDescriptor.Attributes.Type)
                    {
                        injectComponent = null;
                    }
                }
            }
            else
            {
                context.descriptorTypeCache.TryGetValue(injectDescriptor.Type.AssemblyQualifiedName, out injectComponent);
            }

            return context.GetInstance(injectComponent);
        }
    }
}
