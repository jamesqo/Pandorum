using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    internal static class ArrayExtensions
    {
        public static void RemoveAt<T>(this T[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            // uint trick: this is an optimized version of
            // index < 0 || index >= array.Length
            if ((uint)index >= (uint)array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            int nextIndex = index + 1;
            if (nextIndex != array.Length)
            {
                Array.Copy(array, nextIndex, array, index, array.Length - nextIndex);
            }

            // Nil out the last element
            array[array.Length - 1] = default(T);
        }
    }
}
