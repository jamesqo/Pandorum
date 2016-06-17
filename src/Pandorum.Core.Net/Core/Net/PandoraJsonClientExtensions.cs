using Newtonsoft.Json.Linq;
using Pandorum.Core.Net.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    public static class PandoraJsonClientExtensions
    {
        public static Task<JObject> CheckLicensing(this IPandoraJsonClient client)
        {
            return client.CheckLicensing(CheckLicensingOptions.Default);
        }

        public static Task<JObject> PartnerLogin(this IPandoraJsonClient client, string username, string password, string deviceModel, string version)
        {
            return client.PartnerLogin(new PartnerLoginOptions()
            {
                Username = username,
                Password = password,
                DeviceModel = deviceModel,
                Version = version
            });
        }
    }
}
