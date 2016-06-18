using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Numerics
{
    internal static class Hexadecimal
    {
        private static readonly string HexMapping = "0123456789abcdef";

        public static void FromByte(byte value, out char first, out char second)
        {
            first = HexMapping[value >> 4];
            second = HexMapping[value & 0x0f];
        }

        public static byte ToByte(char first, char second)
        {
            return (byte)(ToInt(first) << 4 | ToInt(second));
        }

        private static int ToInt(char digit)
        {
            // Lowercase the letters
            // This will have no effect if the char
            // is numerical, i.e. 9 will stay 9
            digit |= (char)0x20;

            Debug.Assert(
                (digit >= '0' && digit <= '9') ||
                (digit >= 'a' && digit <= 'z'));

            if (digit <= '9')
                return digit - '0';
            return digit - 'a' + 0xa;
        }
    }
}
