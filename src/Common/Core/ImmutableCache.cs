using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    internal static class ImmutableCache
    {
        private static class EmptyArrayHolder<T>
        {
            public static T[] Value { get; } = new T[0];
        }

        private static object s_emptyObject;

        public static object EmptyObject =>
            s_emptyObject ?? (s_emptyObject = new object());

        // Remove once we target a platform supporting Array.Empty<T>
        public static T[] EmptyArray<T>()
        {
            return EmptyArrayHolder<T>.Value;
        }
    }
}
