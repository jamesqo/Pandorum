using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    internal class StationDto
    {
        public string StationToken { get; set; }
        public string StationName { get; set; }
        public DateTimeOffsetDto DateCreated { get; set; }
    }
}
