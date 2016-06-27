using Pandorum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public struct QuickMixStationInfo
    {
        internal QuickMixStationInfo(IEnumerable<IStation> stations)
        {
            if (stations == null)
                throw new ArgumentNullException(nameof(stations));

            Stations = stations;
        }

        public IEnumerable<IStation> Stations { get; }
    }
}
