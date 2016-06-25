using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    internal static class ListExtensions
    {
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            return new ReadOnlyCollection<T>(list);
        }
    }
}
