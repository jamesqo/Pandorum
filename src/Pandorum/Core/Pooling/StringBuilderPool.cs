using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandorum.Core.Pooling
{
    internal class StringBuilderPool : IOwner<StringBuilder>
    {
        private const int DefaultMaxBuffers = 10;
        private const int DefaultMaxBufferSize = 256;

        public static StringBuilderPool Default { get; } = new StringBuilderPool();

        private int _maxOrStoredCount;
        private StringBuilder[] _store;
        private readonly int _maxBufferSize;

        public StringBuilderPool()
            : this(DefaultMaxBuffers, DefaultMaxBufferSize)
        {
        }

        public StringBuilderPool(int maxBuffers, int maxBufferSize)
        {
            _maxOrStoredCount = maxBuffers;
            _maxBufferSize = maxBufferSize;
        }

        public StringBuilder Borrow(int minimumCapacity = 16, bool clear = true)
        {
            if (EnsureStoreInitialized())
            {
                for (int i = 0; i < _maxOrStoredCount; i++)
                {
                    var builder = _store[i];
                    if (builder.Capacity >= minimumCapacity)
                    {
                        // Found one
                        _maxOrStoredCount--;
                        _store.RemoveAt(i);
                        if (clear) builder.Clear();
                        return builder;
                    }
                }
            }

            // We don't have one big enough
            return new StringBuilder(minimumCapacity);
        }

        public Lease<StringBuilder> Lease(int minimumCapacity = 16, bool clear = true)
        {
            return new Lease<StringBuilder>(Borrow(minimumCapacity, clear), this);
        }

        public void Return(StringBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (builder.Capacity > _maxBufferSize) return;
            if (_maxOrStoredCount == _store.Length) return;

            Debug.Assert(_maxOrStoredCount < _store.Length);
            Debug.Assert(_store[_maxOrStoredCount] == null);

            _store[_maxOrStoredCount++] = builder;
        }

        // Returns true if _store was already initialized
        private bool EnsureStoreInitialized()
        {
            if (_store != null)
                return true;

            _store = new StringBuilder[_maxOrStoredCount];
            _maxOrStoredCount = 0;
            return false;
        }
    }
}
