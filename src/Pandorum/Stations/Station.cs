using Newtonsoft.Json;
using Pandorum.Core.DataTransfer;
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
        internal Station(StationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.StationName;
            Token = dto.StationToken;
            DateCreated = dto.DateCreated.ToDateTimeOffset();
        }

        public string Name { get; }
        public DateTimeOffset DateCreated { get; }
        internal string Token { get; }

        public override string ToString() => Name;
    }
}
