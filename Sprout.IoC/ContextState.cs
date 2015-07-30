using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// States of the Context.
    /// 
    /// Created - a Context that hasn't been started
    /// Starting - a Context which had Start invoked on it
    /// Started - a Context that is currently running
    /// Stopping - a Context that had Stop invoked on it
    /// Stopped - a Context that is stopped
    /// </summary>
    public enum ContextState
    {
        Created,
        Starting,
        Started,
        Stopping,
        Stopped
    }
}
