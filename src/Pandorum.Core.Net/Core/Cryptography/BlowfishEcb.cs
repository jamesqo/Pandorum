using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
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
        public static byte[] EncryptBytes(byte[] plaintext, byte[] key)
        {
            return EncryptBytes(
                new ArraySegment<byte>(plaintext),
                new ArraySegment<byte>(key));
        }

        // TODO: Return Lease<T> so this can use array pooling
        public static byte[] EncryptBytes(ArraySegment<byte> plaintext, ArraySegment<byte> key)
        {
            // Blowfish encrypts 8 bytes at a time
            int leftover = plaintext.Count & 7;
            int resultCount = ((plaintext.Count - 1) & ~7) + 8;

            var result = new byte[resultCount];

            var engine = new BlowfishEngine();
            var keyParameter = new KeyParameter(key.Array, key.Offset, key.Count);
            engine.Init(forEncryption: true, parameters: keyParameter);

            int i = plaintext.Offset;
            int j = 0;
            byte[] input = plaintext.Array;

            // Process each block of bytes
            while (i + 7 < plaintext.Count)
            {
                engine.ProcessBlock(input, i, result, j);

                i += 8;
                j += 8;
            }

            Debug.Assert(i + leftover == plaintext.Count);

            if (leftover != 0)
            {
                // Process the stray bytes
                var pooled = ArrayPool<byte>.Shared.Rent(8);
                try
                {
                    // Copy over the leftover data
                    for (int k = 0; k < leftover; k++)
                    {
                        pooled[k] = input[i++];
                    }

                    // Leave the null bytes at the end and process
                    engine.ProcessBlock(pooled, 0, result, j);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(pooled);
                }
            }

            return result;
        }

        public static byte[] EncryptStringToBytes(string plaintext, string key)
        {
            return EncryptStringToBytes(plaintext, key, Encoding.UTF8);
        }

        public static byte[] EncryptStringToBytes(string plaintext, string key, Encoding encoding)
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
                    new ArraySegment<byte>(keyBytes));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(textBytes);
            }
        }
    }
}
