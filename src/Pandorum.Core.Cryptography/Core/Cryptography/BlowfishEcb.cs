using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Pandorum.Core.Numerics;
using Pandorum.Core.Pooling;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandorum.Core.Cryptography
{
    internal static class BlowfishEcb
    {
        public static ArrayLease<byte> EncryptBytes(byte[] plaintext, byte[] key, out int count)
        {
            return EncryptBytes(
                new ArraySegment<byte>(plaintext),
                new ArraySegment<byte>(key),
                out count);
        }

        public static ArrayLease<byte> EncryptBytes(ArraySegment<byte> plaintext, ArraySegment<byte> key, out int count)
        {
            // Blowfish encrypts 8 bytes at a time
            int leftover = plaintext.Count & 7;
            count = ((plaintext.Count - 1) & ~7) + 8;

            var engine = new BlowfishEngine();
            var keyParameter = new KeyParameter(key.Array, key.Offset, key.Count);
            engine.Init(forEncryption: true, parameters: keyParameter);

            var lease = ArrayPool<byte>.Shared.Lease(count);

            try
            {
                int i = plaintext.Offset;
                int j = 0;
                byte[] input = plaintext.Array;
                byte[] output = lease.Array;

                // Process each block of bytes
                while (i + 7 < plaintext.Count)
                {
                    engine.ProcessBlock(input, i, output, j);

                    i += 8;
                    j += 8;
                }

                Debug.Assert(i + leftover == plaintext.Count);

                if (leftover != 0)
                {
                    var unprocessedSegment = new ArraySegment<byte>(input, i, leftover);
                    HandleLeftoverBytes(engine, unprocessedSegment, output, j);
                }
            }
            catch
            {
                lease.Dispose();
                throw;
            }

            return lease;
        }

        public static ArrayLease<byte> EncryptStringToBytes(string plaintext, string key, out int count)
        {
            return EncryptStringToBytes(plaintext, key, Encoding.UTF8, out count);
        }

        public static ArrayLease<byte> EncryptStringToBytes(string plaintext, string key, Encoding encoding, out int count)
        {
            int byteCount = encoding.GetByteCount(plaintext);
            // byteCount needs to be a multiple of 8, so if it
            // isn't round up to the nearest multiple
            byteCount = ((byteCount - 1) & ~7) + 8;

            var textBytes = ArrayPool<byte>.Shared.Rent(byteCount);
            try
            {
                int written = encoding.GetBytes(plaintext, 0, plaintext.Length, textBytes, 0);
                var keyBytes = encoding.GetBytes(key); // TODO: Pool array for key/memoize results?

                return EncryptBytes(
                    new ArraySegment<byte>(textBytes, 0, written),
                    new ArraySegment<byte>(keyBytes),
                    out count);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(textBytes);
            }
        }

        public static string EncryptStringToHex(string plaintext, string key)
        {
            return EncryptStringToHex(plaintext, key, Encoding.UTF8);
        }

        public static string EncryptStringToHex(string plaintext, string key, Encoding encoding)
        {
            int byteCount, charCount;
            using (var lease1 = EncryptStringToBytes(plaintext, key, encoding, out byteCount))
            {
                var bytes = new ArraySegment<byte>(lease1.Array, 0, byteCount);
                using (var lease2 = TranslateBytesToHex(bytes, out charCount))
                {
                    var chars = lease2.Array;
                    return new string(chars, 0, charCount);
                }
            }
        }

        private static void HandleLeftoverBytes(BlowfishEngine engine, ArraySegment<byte> unprocessed, byte[] output, int outputIndex)
        {
            byte[] input = unprocessed.Array;
            int inputIndex = unprocessed.Offset;
            int leftover = unprocessed.Count;

            Debug.Assert(leftover != 0);
            Debug.Assert(outputIndex + 7 < output.Length);

            // Process the stray bytes
            var pooled = ArrayPool<byte>.Shared.Rent(8);
            try
            {
                // Copy over the leftover data
                for (int i = 0; i < leftover; i++)
                {
                    pooled[i] = input[inputIndex++];
                }

                // Leave the null bytes at the end and process
                engine.ProcessBlock(pooled, 0, output, outputIndex);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(pooled);
            }
        }

        private static ArrayLease<char> TranslateBytesToHex(ArraySegment<byte> bytes, out int charCount)
        {
            byte[] buffer = bytes.Array;
            int offset = bytes.Offset;
            
            charCount = bytes.Count * 2;
            var lease = ArrayPool<char>.Shared.Lease(charCount);
            
            try
            {
                var chars = lease.Array;

                int j = 0;
                char left, right;

                for (int i = 0; i < bytes.Count; i++)
                {
                    byte b = buffer[offset + i];
                    Hexadecimal.FromByte(b, out left, out right);
                    chars[j] = left;
                    chars[j + 1] = right;
                    j += 2;
                }

                Debug.Assert(j == charCount);
            }
            catch
            {
                lease.Dispose();
                throw;
            }

            return lease;
        }
    }
}
