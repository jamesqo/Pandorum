using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pandorum.Core.Optional.Tests
{
    public class OptionalBoolTests
    {
        [Fact]
        public void DefaultShouldBeNull()
        {
            var value = default(OptionalBool);
            Assert.False(value.HasValue);
            Assert.False(value.GetValueOrDefault());
            Assert.Throws<InvalidOperationException>(() => value.Value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConversionFromBool(bool value)
        {
            OptionalBool optional = value;
            Assert.True(optional.HasValue);
            Assert.Equal(value, optional.Value);
            Assert.Equal(value, optional.GetValueOrDefault());
        }

        [Theory]
        [MemberData(nameof(NullableBoolData))]
        public void ConversionFromNullableBool(bool? value)
        {
            OptionalBool optional = value;
            Assert.Equal(value.HasValue, optional.HasValue);
            Assert.Equal(value.GetValueOrDefault(), optional.GetValueOrDefault());
        }

        [Theory]
        [MemberData(nameof(ObjectEqualData))]
        public void ObjectEqual(OptionalBool value, object obj)
        {
            Assert.True(value.Equals(obj));
        }

        [Theory]
        [MemberData(nameof(ObjectNotEqualData))]
        public void ObjectNotEqual(OptionalBool value, object obj)
        {
            Assert.False(value.Equals(obj));
        }

        [Theory]
        [MemberData(nameof(HashCodeData))]
        public void HashCode(OptionalBool value, int expected)
        {
            Assert.Equal(expected, value.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(ToStringData))]
        public void ToString(OptionalBool value, string expected)
        {
            Assert.Equal(expected, value.ToString());
        }

        [Fact]
        public void Equatable()
        {
            Assert.True(((OptionalBool)true).Equals(true));
            Assert.True(((OptionalBool)false).Equals(false));
            Assert.False(((OptionalBool)true).Equals(false));

            Assert.False(new OptionalBool().Equals(false));
            Assert.True(default(OptionalBool).Equals(new bool?()));
            Assert.False(new OptionalBool(false).Equals(default(bool?)));
        }

        [Theory]
        [MemberData(nameof(ComparableData))]
        public void Comparable(OptionalBool left, OptionalBool right, int sign)
        {
            Assert.Equal(sign, Math.Sign(left.CompareTo(right)));
            Assert.Equal(-sign, Math.Sign(right.CompareTo(left)));
        }

        public static IEnumerable<object[]> NullableBoolData()
        {
            yield return new object[] { true };
            yield return new object[] { false };
            yield return new object[] { default(bool?) };
        }

        public static IEnumerable<object[]> ObjectEqualData()
        {
            yield return new object[] { new OptionalBool(true), true };
            yield return new object[] { new OptionalBool(false), false };
            yield return new object[] { new OptionalBool(true), new bool?(true) };
            yield return new object[] { default(OptionalBool), new bool?() };
            yield return new object[] { default(OptionalBool), null };
            yield return new object[] { default(OptionalBool), default(OptionalBool) };
            yield return new object[] { new OptionalBool(true), new OptionalBool(true) };
            yield return new object[] { new OptionalBool(false), new OptionalBool(false) };
        }

        public static IEnumerable<object[]> ObjectNotEqualData()
        {
            yield return new object[] { new OptionalBool(true), false };
            yield return new object[] { new OptionalBool(true), new bool?(false) };
            yield return new object[] { default(OptionalBool), false };
            yield return new object[] { new OptionalBool(true), new object() };
            yield return new object[] { new OptionalBool(false), new OptionalBool(true) };
        }

        public static IEnumerable<object[]> HashCodeData()
        {
            yield return new object[] { new OptionalBool(true), new bool?(true).GetHashCode() };
            yield return new object[] { new OptionalBool(false), new bool?(false).GetHashCode() };
            yield return new object[] { new OptionalBool(), new bool?().GetHashCode() };
        }

        public static IEnumerable<object[]> ToStringData()
        {
            yield return new object[] { new OptionalBool(true), new bool?(true).ToString() };
            yield return new object[] { new OptionalBool(false), new bool?(false).ToString() };
            yield return new object[] { new OptionalBool(), new bool?().ToString() };
        }

        public static IEnumerable<object[]> ComparableData()
        {
            yield return new object[] { default(OptionalBool), new OptionalBool(), 0 };
            yield return new object[] { new OptionalBool(true), new OptionalBool(true), 0 };
            yield return new object[] { new OptionalBool(false), new OptionalBool(false), 0 };
            yield return new object[] { new OptionalBool(), new OptionalBool(true), -1 };
            yield return new object[] { new OptionalBool(), new OptionalBool(false), -1 };
            yield return new object[] { new OptionalBool(true), new OptionalBool(false), 1 };
        }
    }
}
