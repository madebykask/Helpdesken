using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Common.Extensions
{
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _hash;

        #region ctor()

        public LambdaEqualityComparer(Func<T, string> toString)
            : this((t1, t2) => toString(t1) == toString(t2), t => toString(t).GetHashCode())
        {
        }

        public LambdaEqualityComparer(Func<T, T, bool> comparer)
            : this(comparer, t => t.GetHashCode())
        {
        }

        public LambdaEqualityComparer(Func<T, T, bool> comparer, Func<T, int> hash)
        {
            _comparer = comparer;
            _hash = hash;
        }

        #endregion

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hash(obj);
        }
    }
}