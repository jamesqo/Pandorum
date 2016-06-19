using Newtonsoft.Json.Linq;
using Pandorum.Core.Net.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    public interface IPandoraJsonClient : IDisposable
    {
        Task<JObject> CheckLicensing();
        Task<JObject> PartnerLogin(PartnerLoginOptions options);
        Task<JObject> UserLogin(UserLoginOptions options);
    }
}
