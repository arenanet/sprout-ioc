using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// An IEqualityComparer that can have only one instance of a particular type.
    /// </summary>
    internal class TypeUniqueEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        /// <summary>
        /// Returns true if the given two objects have the same type.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            if (x == y)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            return x.GetType().AssemblyQualifiedName.Equals(y.GetType().AssemblyQualifiedName);
        }

        /// <summary>
        /// Returns the unique Type hashcode.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return obj.GetType().AssemblyQualifiedName.GetHashCode();
        }
    }
}
