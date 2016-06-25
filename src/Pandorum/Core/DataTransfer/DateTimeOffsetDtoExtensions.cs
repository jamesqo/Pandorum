using Pandorum.Core.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer
{
    internal static class DateTimeOffsetDtoExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTimeOffsetDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return DateTimeHelpers.FromUnixTimeMillis(dto.Time);
        }
    }
}
