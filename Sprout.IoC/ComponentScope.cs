using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// The scopes available for components.
    /// 
    /// Context - one per Context
    /// Injection - one per injection in Context
    /// </summary>
    public enum ComponentScope
    {
        Context,
        Injection
    }
}
