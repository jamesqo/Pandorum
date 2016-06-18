using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pandorum.Core.Pooling.Tests
{
    public class StringBuilderPoolTests
    {
        [Fact]
        public void Default()
        {
            Assert.NotNull(StringBuilderPool.Default);
            Assert.Same(StringBuilderPool.Default, StringBuilderPool.Default);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(100, 0)]
        [InlineData(-1, 19)]
        [InlineData(123, -5)]
        [InlineData(-9, -10)]
        public void ConstructorShouldThrowForNonPositiveValues(int maxBuffers, int maxBufferSize)
        {
            bool invalidMaxBuffers = maxBuffers <= 0;
            string paramName = invalidMaxBuffers ? "maxBuffers" : "maxBufferSize";
            Assert.Throws<ArgumentOutOfRangeException>(paramName, () => new StringBuilderPool(maxBuffers, maxBufferSize));
        }

        [Fact]
        public void SameInstanceShouldBeReturned()
        {
            var pool = new StringBuilderPool();
            var sb = pool.Borrow();
            Assert.NotNull(sb);

            pool.Return(sb);
            var sb2 = pool.Borrow();
            Assert.Same(sb, sb2);
        }

        [Fact]
        public void InstanceShouldNotBeStoredIfTooLarge()
        {
            var pool = new StringBuilderPool(maxBuffers: 10, maxBufferSize: 20);
            var sb = new StringBuilder(21);
            pool.Return(sb);
            var sb2 = pool.Borrow();
            Assert.NotSame(sb, sb2);

            var sb3 = new StringBuilder(20);
            pool.Return(sb3);
            var sb4 = pool.Borrow();
            Assert.Same(sb3, sb4);
        }

        [Fact]
        public void ThrowForNullInReturn()
        {
            Assert.Throws<ArgumentNullException>("builder", () => StringBuilderPool.Default.Return(null));
        }

        [Fact]
        public void ShouldOnlyStoreUpToMaxBuffers()
        {
            var pool = new StringBuilderPool(maxBuffers: 10, maxBufferSize: 100);
            var returned = new List<StringBuilder>();

            for (int i = 0; i < 20; i++)
            {
                var sb = new StringBuilder(50);
                returned.Add(sb);
                pool.Return(sb);
            }

            var stack1 = new Stack<StringBuilder>();
            var stack2 = new Stack<StringBuilder>();

            for (int i = 0; i < 10; i++)
            {
                stack1.Push(pool.Borrow(50));
            }

            for (int i = 0; i < 10; i++)
            {
                stack2.Push(pool.Borrow(50));
            }

            // The first 10 StringBuilders
            // should have been stored in the pool
            for (int i = 0; i < 10; i++)
            {
                var left = returned[i];
                var right = stack1.Pop();
                Assert.Same(left, right);
                Assert.DoesNotContain(left, stack2);
            }

            // The next 10 shouldn't
            for (int i = 11; i < 20; i++)
            {
                var sb = returned[i];
                Assert.DoesNotContain(sb, stack1);
                Assert.DoesNotContain(sb, stack2);
            }
        }

        [Fact]
        public void StressTest()
        {
            var pool = new StringBuilderPool();
            for (int i = 0; i < 100; i++)
                pool.Borrow();
            for (int i = 0; i < 100; i++)
                pool.Return(new StringBuilder());
        }
    }
}
