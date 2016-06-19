using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pandorum.Core.Optional.Tests
{
    // TODO: Add more tests, including IEquatable/etc.

    public class FourOptionalBoolsTests
    {
        [Fact]
        public void DefaultValuesShouldBeNull()
        {
            var value = default(FourOptionalBools);
            ValidateState(value, null, null, null, null);
        }

        [Theory]
        [MemberData(nameof(SampleData))]
        public void TestIntegrity(OptionalBool item1, OptionalBool item2, OptionalBool item3, OptionalBool item4)
        {
            var value = new FourOptionalBools(item1, item2, item3, item4);
            ValidateState(value, item1, item2, item3, item4);

            value = value.WithFirst(item2);
            ValidateState(value, item2, item2, item3, item4);

            value = value.WithFourth(item1);
            ValidateState(value, item2, item2, item3, item1);

            value = value.WithSecond(item4);
            ValidateState(value, item2, item4, item3, item1);

            value = value.WithThird(item3);
            ValidateState(value, item2, item4, item3, item1);

            value = value.WithThird(item2).WithFourth(item2);
            ValidateState(value, item2, item4, item2, item2);
        }

        [Fact]
        public void TestImmutability()
        {
            var value = default(FourOptionalBools);

            value.WithFourth(true);
            Assert.Null((bool?)value.Fourth);

            value = value.WithFourth(true);
            Assert.True(value.Fourth);
        }

        public static IEnumerable<object[]> SampleData()
        {
            foreach (var tuple in SampleDataCore())
            {
                yield return new object[] { tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4 };
            }
        }

        // TODO: C# 7 will get built-in support for tuples, use those
        private static IEnumerable<OptionalBoolQuartuple> SampleDataCore()
        {
            return SelectStates(s1 =>
            {
                return SelectStates(s2 =>
                {
                    return SelectStates(s3 =>
                    {
                        return SelectStates(s4 =>
                        {
                            return new OptionalBoolQuartuple(
                                GetOptionalBoolFromState(s1),
                                GetOptionalBoolFromState(s2),
                                GetOptionalBoolFromState(s3),
                                GetOptionalBoolFromState(s4));
                        });
                    });
                });
            })
            .Flatten().Flatten().Flatten();
        }

        private static IEnumerable<T> SelectStates<T>(Func<OptionalBoolState, T> func)
        {
            foreach (var obj in Enum.GetValues(typeof(OptionalBoolState)))
            {
                yield return func((OptionalBoolState)obj);
            }
        }

        private static OptionalBool GetOptionalBoolFromState(OptionalBoolState state)
        {
            if (state == OptionalBoolState.Null)
                return default(OptionalBool);
            if (state == OptionalBoolState.True)
                return new OptionalBool(true);
            return new OptionalBool(false);
        }

        private static void ValidateState(FourOptionalBools value, OptionalBool first, OptionalBool second, OptionalBool third, OptionalBool fourth)
        {
            Assert.Equal(first, value.First);
            Assert.Equal(second, value.Second);
            Assert.Equal(third, value.Third);
            Assert.Equal(fourth, value.Fourth);
        }
    }
}
