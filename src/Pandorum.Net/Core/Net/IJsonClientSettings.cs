using Pandorum.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    public interface IJsonClientSettings
    {
        string Endpoint { get; set; }
        IPartnerInfo PartnerInfo { get; set; }
        long SyncTimestamp { get; set; }
    }
}
