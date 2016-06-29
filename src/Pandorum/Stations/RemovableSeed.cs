using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    internal class RemovableSeed : IRemovableSeed
    {
        public RemovableSeed(RemovableSeedDto dto, SeedType seedType)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            SeedId = dto.SeedId;
            SeedType = seedType;
        }

        public string SeedId { get; }
        public SeedType SeedType { get; }
    }
}
