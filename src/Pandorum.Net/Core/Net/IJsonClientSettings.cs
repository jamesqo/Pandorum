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

        // Received from partner login
        long SyncTimestamp { get; set; }
        string AuthToken { get; set; } // replaced by the user authToken upon user login
        string PartnerId { get; set; }

        // Received from user login
        string UserId { get; set; }
    }
}
