using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Pooling
{
    public interface IOwner<in T>
    {
        void Return(T obj);
    }
}
