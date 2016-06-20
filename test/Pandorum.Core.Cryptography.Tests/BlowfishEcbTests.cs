using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pandorum.Core.Cryptography.Tests
{
    public class BlowfishEcbTests
    {
        [Theory]
        [MemberData(nameof(EncryptDecryptData))]
        public void BytesEncryption(string plaintext, string key, string hex)
        {
            var textBytes = Encoding.UTF8.GetBytes(plaintext);
            var keyBytes = Encoding.UTF8.GetBytes(key);

            int count;
            using (var lease = BlowfishEcb.EncryptBytes(textBytes, keyBytes, out count))
            {
                var outputHex = BitConverter
                    .ToString(lease.Array, 0, count)
                    .Replace("-", string.Empty)
                    .ToLowerInvariant();

                Assert.Equal(hex, outputHex);
            }
        }

        [Theory]
        [MemberData(nameof(EncryptDecryptData))]
        public void StringEncryption(string plaintext, string key, string hex)
        {
            Assert.Equal(hex, BlowfishEcb.EncryptStringToHex(plaintext, key));
        }

        public static IEnumerable<object[]> EncryptDecryptData()
        {
            // Everything divisible by 8 (no padding)
            yield return new object[] { "foobarba", "12345678", "5cded4361825e53c" };
            yield return new object[] { "01234567", "16letter", "6884c9aa2ea67ff3" };
            yield return new object[] { "0123456789ABCDEF", "16letterkey55555", "947754b3ea0120a6f7c8c218ac623983" };

            // Strings aren't multiples of 8; need padding
            yield return new object[] { "garble102934", "f44h389", "fdbbbf018865c902f55b311cb5de9b33" };
            // 1/7-byte strings to catch off-by-one errors
            yield return new object[] { "b", "a", "897c4fb958fbd095" };
            yield return new object[] { "bbbbbbb", "aaaaaaa", "820ff7b845e16731" };

            // TODO: Add tests for UTF-8 strings
            // Note that each non-ASCII char will be represented as
            // 2+ bytes in binary
        }
    }
}
