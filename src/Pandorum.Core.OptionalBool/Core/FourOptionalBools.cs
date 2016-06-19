using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    public struct FourOptionalBools : IEquatable<FourOptionalBools>
    {
        private readonly byte _data;

        public FourOptionalBools(OptionalBool first, OptionalBool second, OptionalBool third, OptionalBool fourth)
        {
            int data = (first._data << 6) |
                (second._data << 4) |
                (third._data << 2) |
                fourth._data;
            _data = (byte)data;
        }

        private FourOptionalBools(byte data)
        {
            _data = data;
        }

        public OptionalBool First => new OptionalBool((byte)(_data >> 6));
        public OptionalBool Second => new OptionalBool((byte)(_data >> 4 & 0x03));
        public OptionalBool Third => new OptionalBool((byte)(_data >> 2 & 0x03));
        public OptionalBool Fourth => new OptionalBool((byte)(_data & 0x03));

        public override bool Equals(object obj)
        {
            return obj is FourOptionalBools &&
                Equals((FourOptionalBools)obj);
        }

        public bool Equals(FourOptionalBools other)
        {
            return _data == other._data;
        }

        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        public FourOptionalBools WithFirst(OptionalBool value)
        {
            int data = (_data & ~0xc0) | (value._data << 6);
            return new FourOptionalBools((byte)data);
        }

        public FourOptionalBools WithSecond(OptionalBool value)
        {
            int data = (_data & ~0x30) | (value._data << 4);
            return new FourOptionalBools((byte)data);
        }

        public FourOptionalBools WithThird(OptionalBool value)
        {
            int data = (_data & ~0x0c) | (value._data << 2);
            return new FourOptionalBools((byte)data);
        }

        public FourOptionalBools WithFourth(OptionalBool value)
        {
            int data = (_data & ~0x03) | value._data;
            return new FourOptionalBools((byte)data);
        }
    }
}
