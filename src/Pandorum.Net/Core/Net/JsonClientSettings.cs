using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pandorum.Net.Authentication;

namespace Pandorum.Core.Net
{
    public class JsonClientSettings : IJsonClientSettings
    {
        public string Endpoint { get; set; }
        public IPartnerInfo PartnerInfo { get; set; }

        public long SyncTimestamp { get; set; }
        public string AuthToken { get; set; }
        public string PartnerId { get; set; }
    }
}
