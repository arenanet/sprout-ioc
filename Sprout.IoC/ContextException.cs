using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// Base Context exception.
    /// </summary>
    public abstract class ContextException : Exception
    {
        public ContextException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
