using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Options.Stations
{
    public class ShareStationOptions
    {
        public string StationId { get; set; }
        public string StationToken { get; set; }
        public IEnumerable<string> Emails { get; set; }
    }
}
