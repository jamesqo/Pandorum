using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Pooling
{
    public static class ArrayPoolExtensions
    {
        public static ArrayLease<T> Lease<T>(this ArrayPool<T> pool, int minimumLength)
        {
            return new ArrayLease<T>(pool.Rent(minimumLength), pool);
        }
    }
}
