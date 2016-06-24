using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    internal static class ArrayExtensions
    {
        // Array.AsReadOnly is not exposed in .NET Core
        public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            return new ReadOnlyCollection<T>(array);
        }
    }
}
