using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Time
{
    // TODO: We can remove this once we move to .NET 4.6,
    // as that adds built-in support for Unix time

    // TODO: Make this an internal class/move to another
    // assembly

    public static class DateTimeOffsetExtensions
    {
        private static readonly DateTimeOffset s_unixEpoch =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static long ToUnixTime(this DateTimeOffset offset)
        {
            return (long)(offset - s_unixEpoch).TotalSeconds;
        }
    }
}
