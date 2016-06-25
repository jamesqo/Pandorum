using Newtonsoft.Json;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Station
    {
        internal Station(StationDto)
        [JsonConstructor]
        private Station(string stationToken, string stationName, DateTimeOffset dateCreated)
        {
            Debug.Assert(stationToken != null);
            Debug.Assert(stationName != null);

            Name = stationName;
            Token = stationToken;
            DateCreated = dateCreated;
        }

        public string Name { get; }
        public DateTimeOffset DateCreated { get; }

        internal string Token { get; }

        public override string ToString() => Name;
    }
}
