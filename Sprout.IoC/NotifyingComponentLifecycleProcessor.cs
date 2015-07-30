using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// A component lifecycle processor that notifies the Components of the lifecycle.
    /// </summary>
    internal class NotifyingComponentLifecycleProcessor : IComponentLifecycleProcessor
    {
        /// <summary>
        /// Notifies the given component when its Injections are complete.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="descriptor"></param>
        public void OnStart(Context context, object component, ComponentDescriptor descriptor)
        {
            foreach (MemberInfo member in descriptor.Type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (member is MethodInfo)
                {
                    if (member.GetCustomAttribute<OnStartAttribute>() != null)
                    {
                        ((MethodInfo)member).Invoke(component, new object[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Notifies the given component when the Context stops.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="descriptor"></param>
        public void OnStop(Context context, object component, ComponentDescriptor descriptor)
        {
            foreach (MemberInfo member in descriptor.Type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (member is MethodInfo)
                {
                    if (member.GetCustomAttribute<OnStopAttribute>() != null)
                    {
                        ((MethodInfo)member).Invoke(component, new object[0]);
                    }
                }
            }
        }
    }
}
