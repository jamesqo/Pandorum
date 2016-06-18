using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Pooling.Tests.Mocks
{
    public class FakeOwner<T> : IOwner<T>
    {
        public T ReturnedObject { get; private set; }
        public int ReturnCount { get; private set; }

        public void Return(T obj)
        {
            ReturnedObject = obj;
            ReturnCount++;
        }
    }
}
