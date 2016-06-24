using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Time
{
    // TODO: We can remove this once we move to .NET 4.6,
    // as that adds built-in support for Unix time

    internal static class DateTimeHelpers
    {
        public static DateTimeOffset UnixEpoch { get; } =
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static DateTimeOffset FromUnixTime(long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(this DateTimeOffset offset)
        {
            return (long)(offset - UnixEpoch).TotalSeconds;
        }
    }
}
