using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    internal class SeedRemover : ISeedRemover
    {
        public SeedRemover(string seedId)
        {
            if (seedId == null)
                throw new ArgumentNullException(nameof(seedId));

            SeedId = seedId;
        }

        public string SeedId { get; }
    }
}
