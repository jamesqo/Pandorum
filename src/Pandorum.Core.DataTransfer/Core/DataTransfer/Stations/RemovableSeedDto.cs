using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class RemovableSeedDto
    {
        public string SeedId { get; set; }
        public string MusicToken { get; set; } // NOTE: different from the musicToken in I{Addable,Creatable}Seed
    }
}
