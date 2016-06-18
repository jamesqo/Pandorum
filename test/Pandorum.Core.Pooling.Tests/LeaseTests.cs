using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pandorum.Core.Pooling.Tests
{
    public class IntLeaseTests : LeaseTests<int>
    {
        static IntLeaseTests()
        {
            TestValues.Add(3);
            TestValues.Add(0);
            TestValues.Add(int.MaxValue);
            TestValues.Add(int.MinValue);
        }
    }

    public class NullableLeaseTests : LeaseTests<int?>
    {
        static NullableLeaseTests()
        {
            TestValues.Add(default(int?));
            TestValues.Add(0);
            TestValues.Add(111111);
        }
    }

    public class StringLeaseTests : LeaseTests<string>
    {
        static StringLeaseTests()
        {
            TestValues.Add(null);
            TestValues.Add(string.Empty);
            TestValues.Add("baz-foo-bar");
            TestValues.Add("\ud800");
        }
    }

    public abstract class LeaseTests<T>
    {
        protected static List<T> TestValues { get; } = new List<T>();

        [Fact]
        public void DefaultValueShouldBeOk()
        {
            var lease = CreateLease(default(T));
        }

        [Theory]
        [MemberData(nameof(SampleValues))]
        public void ItemShouldBeSameAsPassedInFromConstructor(T value)
        {
            var lease = CreateLease(value);
            Assert.Equal(value, lease.Item);
        }

        [Fact]
        public void NullOwnerShouldThrowArgumentNull()
        {
            Assert.Throws<ArgumentNullException>("owner", () => new Lease<T>(default(T), owner: null));
        }

        [Theory]
        [MemberData(nameof(SampleValues))]
        public void ReturnShouldBeCalledOnce(T value)
        {
            var owner = CreateFakeOwner();
            var lease = new Lease<T>(value, owner);
            lease.Dispose();
            Assert.Equal(1, owner.ReturnCount);
            Assert.Equal(value, owner.ReturnedObject);

            // Return should not be called again the 2nd time
            lease.Dispose();
            Assert.Equal(1, owner.ReturnCount);
        }

        [Theory]
        [MemberData(nameof(SampleValues))]
        public void ItemShouldBeDefaultAfterDispose(T value)
        {
            var lease = CreateLease(value);
            lease.Dispose();
            Assert.Equal(default(T), lease.Item);
        }

        public static IEnumerable<object[]> SampleValues()
        {
            foreach (T value in TestValues)
            {
                yield return new object[] { value };
            }
        }

        private Lease<T> CreateLease(T value)
        {
            return new Lease<T>(value, CreateFakeOwner());
        }

        private FakeOwner<T> CreateFakeOwner()
        {
            return new FakeOwner<T>();
        }
    }
}
