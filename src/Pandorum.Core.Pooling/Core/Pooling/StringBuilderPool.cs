using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandorum.Core.Pooling
{
    public class StringBuilderPool : IOwner<StringBuilder>
    {
        private const int DefaultMaxBuffers = 10;
        private const int DefaultMaxBufferSize = 256;

        public static StringBuilderPool Default { get; } = new StringBuilderPool();

        private int _storeCount; // set to max # of buffers if _store is null
        // otherwise number of StringBuilders in _store
        private StringBuilder[] _store;
        private readonly int _maxBufferSize;

        public StringBuilderPool()
            : this(DefaultMaxBuffers, DefaultMaxBufferSize)
        {
        }

        public StringBuilderPool(int maxBuffers, int maxBufferSize)
        {
            if (maxBuffers <= 0 || maxBufferSize <= 0)
                throw new ArgumentOutOfRangeException(maxBuffers <= 0 ? nameof(maxBuffers) : nameof(maxBufferSize));

            _storeCount = maxBuffers;
            _maxBufferSize = maxBufferSize;
        }

        public StringBuilder Borrow(int minCapacity = 16, bool clear = true)
        {
            EnsureStoreInitialized();

            for (int i = _storeCount - 1; i >= 0; i--)
            {
                var builder = _store[i];
                if (builder.Capacity >= minCapacity)
                {
                    // Found one
                    _storeCount--;

                    // Remove it from the array
                    if (i != _storeCount)
                        Array.Copy(_store, i + 1, _store, i, _storeCount - i);
                    _store[_storeCount] = null;

                    if (clear)
                        builder.Clear();
                    return builder;
                }
            }

            // We don't have one big enough
            return new StringBuilder(minCapacity);
        }

        public Lease<StringBuilder> Lease(int minCapacity = 16, bool clear = true)
        {
            return new Lease<StringBuilder>(Borrow(minCapacity, clear), this);
        }

        public void Return(StringBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (builder.Capacity > _maxBufferSize) return;

            EnsureStoreInitialized();
            if (_storeCount == _store.Length) return;

            Debug.Assert(_storeCount < _store.Length && _store[_storeCount] == null);

            _store[_storeCount++] = builder;
        }

        private void EnsureStoreInitialized()
        {
            if (_store == null)
            {
                _store = new StringBuilder[_storeCount];
                _storeCount = 0;
            }
        }
    }
}
