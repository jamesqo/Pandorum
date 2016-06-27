using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    internal class RemovableSeed : IRemovableSeed
    {
        public RemovableSeed(string seedId, SeedType seedType)
        {
            if (seedId == null)
                throw new ArgumentNullException(nameof(seedId));

            SeedId = seedId;
            SeedType = seedType;
        }

        public string SeedId { get; }
        public SeedType SeedType { get; }
    }
}
