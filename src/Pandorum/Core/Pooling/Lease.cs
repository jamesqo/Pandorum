using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandorum.Core.Pooling
{
    // Provides a more user-friendly way to
    // lease objects from object pools (IOwners),
    // by letting you use the using statement.
    internal struct Lease<T> : IDisposable
    {
        private T _leased;
        private IOwner<T> _owner;

        public Lease(T leased, IOwner<T> owner)
        {
            _leased = leased;
            _owner = owner;
        }

        public T Item => _leased;

        public void Dispose()
        {
            if (_owner != null)
            {
                try
                {
                    _owner.Return(_leased);
                }
                finally
                {
                    _owner = null;
                    _leased = default(T);
                }
            }
        }
    }
}
