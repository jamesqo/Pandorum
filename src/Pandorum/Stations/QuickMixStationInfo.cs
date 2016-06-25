using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public struct QuickMixStationInfo
    {
        internal QuickMixStationInfo(string[] stationIds)
        {
            StationIds = stationIds?.AsReadOnly();
        }

        public IEnumerable<string> StationIds { get; }
    }
}
