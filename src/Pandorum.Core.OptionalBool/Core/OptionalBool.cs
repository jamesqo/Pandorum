using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    // TODO: FourOptionalBools, FiveOptionalBools both in 1 byte

    // Just like bool?, except slimmed down to one byte
    public struct OptionalBool : IEquatable<OptionalBool>, IComparable<OptionalBool>
    {
        private const byte Null = 0;
        private const byte False = 1;
        private const byte True = 2;

        // Least significant bit is set if true
        // The next one up is set if we have a value
        internal readonly byte _data;

        public OptionalBool(bool value)
        {
            _data = value ? True : False;
        }

        public OptionalBool(bool? value)
        {
            _data = value.HasValue ?
                value.GetValueOrDefault() ?
                True : False : Null;
        }

        internal OptionalBool(byte data)
        {
            _data = data;
        }

        public bool HasValue => _data != Null;
        public bool GetValueOrDefault() => _data == True;
        public bool Value => ((bool?)this).Value;

        // Operators

        public static implicit operator bool?(OptionalBool value)
        {
            // Unfortunately we have to branch here since we don't
            // have access to Nullable's internals
            // If we did it could be:
            // return new bool? { value = value.IsTrue, hasValue = value.HasValue };

            return value.HasValue ?
                new bool?(value.GetValueOrDefault()) :
                default(bool?);
        }

        public static implicit operator OptionalBool(bool? value)
        {
            return new OptionalBool(value);
        }

        public static explicit operator bool(OptionalBool value)
        {
            return value.Value;
        }

        public static implicit operator OptionalBool(bool value)
        {
            return new OptionalBool(value);
        }

        public int CompareTo(OptionalBool other)
        {
            return _data.CompareTo(other._data);
        }

        public override bool Equals(object obj)
        {
            /*
            if (HasValue ^ obj != null)
            {
                return false;
            }
            return obj is OptionalBool &&
                Equals((OptionalBool)obj);
            */
            // Above will return true if obj is a 'null' OptionalBool
            if (obj is OptionalBool)
            {
                return Equals((OptionalBool)obj);
            }
            return !HasValue && obj == null;
        }

        public bool Equals(OptionalBool other)
        {
            return _data == other._data;
        }

        public override int GetHashCode()
        {
            return GetValueOrDefault() ? 1 : 0;
        }

        public override string ToString()
        {
            return HasValue ? GetValueOrDefault().ToString() : string.Empty;
        }
    }
}
