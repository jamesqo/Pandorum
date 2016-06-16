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

        private int _maxOrStoreCount;
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

            _maxOrStoreCount = maxBuffers;
            _maxBufferSize = maxBufferSize;
        }

        public StringBuilder Borrow(int minCapacity = 16, bool clear = true)
        {
            if (_store == null)
                InitializeStore();
            else
            {
                for (int i = 0; i < _maxOrStoreCount; i++)
                {
                    var builder = _store[i];
                    if (builder.Capacity >= minCapacity)
                    {
                        // Found one
                        _maxOrStoreCount--;

                        // Remove it from the array
                        if (i != _maxOrStoreCount)
                            Array.Copy(_store, i + 1, _store, i, _maxOrStoreCount - i);
                        _store[_maxOrStoreCount] = null;

                        if (clear)
                            builder.Clear();
                        return builder;
                    }
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
            if (_store == null) InitializeStore();
            else if (_maxOrStoreCount == _store.Length) return;

            Debug.Assert(_maxOrStoreCount < _store.Length);
            Debug.Assert(_store[_maxOrStoreCount] == null);

            _store[_maxOrStoreCount++] = builder;
        }

        private void InitializeStore()
        {
            Debug.Assert(_store == null);

            _store = new StringBuilder[_maxOrStoreCount];
            _maxOrStoreCount = 0;
        }
    }
}
