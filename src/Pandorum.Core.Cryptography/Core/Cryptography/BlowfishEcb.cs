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
    public static class BlowfishEcb
    {
        // Decryption
        // TODO: Maybe separate this out into a new file, like BlowfishEcb.Decryption.cs?

        public static ArrayLease<byte> DecryptBytes(byte[] ciphertext, byte[] key, out int count)
        {
            return DecryptBytes(
                new ArraySegment<byte>(ciphertext),
                new ArraySegment<byte>(key),
                out count);
        }

        public static ArrayLease<byte> DecryptBytes(ArraySegment<byte> ciphertext, ArraySegment<byte> key, out int count)
        {
            // Blowfish processes things in blocks of 8, so
            // it doesn't make sense to have a non-multiple
            // of 8 length ciphertext (TODO: right?)
            if ((ciphertext.Count & 7) != 0) // ciphertext.Count % 8 != 0
            {
                // TODO: Have a Strings.resx file for this
                // Does project.json support that? / What benefits will that yield?
                throw new ArgumentException(
                    message: "The length of the ciphertext needs to be divisible by 8.",
                    paramName: nameof(ciphertext));
            }

            // Since there's guaranteed to be no leftover,
            // the count is just ciphertext.Count
            count = ciphertext.Count;

            var engine = new BlowfishEngine();
            var keyParameter = new KeyParameter(key.Array, key.Offset, key.Count);
            engine.Init(forEncryption: false, parameters: keyParameter);

            var lease = ArrayPool<byte>.Shared.Lease(count);

            try
            {
                var encryptBuffer = ciphertext.Array;
                var decryptBuffer = lease.Array;

                for (int i = 0; i < count; i += 8)
                {
                    engine.ProcessBlock(encryptBuffer, ciphertext.Offset + i, decryptBuffer, i);
                }
            }
            catch
            {
                lease.Dispose();
                throw;
            }

            return lease;
        }

        // TODO: Maybe public DecryptBytesToString here?

        public static string DecryptHexToString(string ciphertext, string key)
        {
            return DecryptHexToString(ciphertext, key, Encoding.UTF8);
        }

        public static string DecryptHexToString(string ciphertext, string key, Encoding encoding)
        {
            // First translate the ciphertext from hex -> byte array
            // Throw if the string isn't divisible by 2, since bytes are 2 hex digits
            if ((ciphertext.Length & 1) != 0) // ciphertext.Length % 2 != 0
            {
                // TODO: Strings.resx if it's worth it (see notes above)
                throw new ArgumentException(
                    message: "The ciphertext needs to be hex-encoded and have a length divisible by 2.",
                    paramName: nameof(ciphertext));
            }

            int bufferLength = ciphertext.Length / 2;
            var rented = ArrayPool<byte>.Shared.Rent(bufferLength);

            try
            {
                Debug.Assert(bufferLength <= rented.Length);

                // Do the translation via Hexadecimal.ToByte
                int i = 0, j = 0;
                while (i < bufferLength)
                {
                    rented[i] = Hexadecimal.ToByte(ciphertext[j], ciphertext[j + 1]);

                    i += 1;
                    j += 2;
                }

                Debug.Assert(i == bufferLength && j == i * 2);

                // Now call DecryptBytes with the key/encrypted buffer
                var encryptedBytes = new ArraySegment<byte>(rented, 0, bufferLength);
                var keyBytes = new ArraySegment<byte>(encoding.GetBytes(key)); // TODO: Pool/memoize this?

                int decryptedCount;
                using (var lease = DecryptBytes(encryptedBytes, keyBytes, out decryptedCount))
                {
                    return encoding.GetString(lease.Array, 0, decryptedCount);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        // Encryption
        // TODO: Maybe separate this out into a new file, like BlowfishEcb.Encryption.cs?

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
            count = (plaintext.Count + 7) & ~7; // branchless version of: n - (n % 8) + 8

            // if there's leftover we'll need another extra 8 bytes to hold the input
            // note that this should not be part of the outputted array,
            // so store it in a new variable
            int capacity = count + ((leftover + 7) & ~7);

            Debug.Assert(leftover == 0 ^ capacity != plaintext.Count);

            var engine = new BlowfishEngine();
            var keyParameter = new KeyParameter(key.Array, key.Offset, key.Count);
            engine.Init(forEncryption: true, parameters: keyParameter);

            var lease = ArrayPool<byte>.Shared.Lease(capacity);

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

                Debug.Assert(
                    i + leftover == plaintext.Count &&
                    leftover >= 0 &&
                    leftover < 8);

                if (leftover != 0)
                {
                    // Handle the leftover input bytes

                    Debug.Assert(
                        count > plaintext.Count &&
                        (count % 8) == 0 &&
                        capacity == count + 8);

                    // Copy over the remaining input data to the end of the output array
                    int inputIndex = i, outputIndex = j + 8;
                    while (inputIndex < plaintext.Count)
                    {
                        output[outputIndex++] = input[inputIndex++];
                    }

                    // Since this is a rented array, clear any extraneous data at the end
                    // Use a regular while loop, since it's less than 8 bytes
                    Debug.Assert(
                        outputIndex <= capacity &&
                        capacity - outputIndex < 8);
                    while (outputIndex < capacity)
                    {
                        output[outputIndex++] = 0;
                    }

                    engine.ProcessBlock(output, j + 8, output, j);
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
            byteCount = (byteCount + 7) & ~7;

            var textBytes = ArrayPool<byte>.Shared.Rent(byteCount);
            try
            {
                int written = encoding.GetBytes(plaintext, 0, plaintext.Length, textBytes, 0);
                Debug.Assert(written <= byteCount && byteCount - written < 8);

                // Rented arrays are not always clear, so zero
                // out any bytes between written and byteCount
                for (int i = byteCount - 1; i >= written; i--)
                {
                    textBytes[i] = 0;
                }

                var keyBytes = encoding.GetBytes(key); // TODO: Pool array for key/memoize results?

                var result = EncryptBytes(
                    new ArraySegment<byte>(textBytes, 0, byteCount),
                    new ArraySegment<byte>(keyBytes),
                    out count);

                Debug.Assert(count == byteCount,
                    "Since we made sure to round up the byte count to " +
                    "a multiple of 8, the end count should be the same");

                return result;
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
