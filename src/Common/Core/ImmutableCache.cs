using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core
{
    internal static class ImmutableCache
    {
        private static object s_emptyObject;

        public static object EmptyObject =>
            s_emptyObject ?? (s_emptyObject = new object());
    }
}
