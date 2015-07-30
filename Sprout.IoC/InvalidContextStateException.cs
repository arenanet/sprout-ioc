using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An exception that is thrown when methods are invoked on the Context but the methods expect a certain state.
    /// </summary>
    public class InvalidContextStateException : ContextException
    {
        public InvalidContextStateException(ContextState expectedState, ContextState currentState) : base("Expected: " + expectedState + ", but current state is: " + currentState)
        {

        }
    }
}
