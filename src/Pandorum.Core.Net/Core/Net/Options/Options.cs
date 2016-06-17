using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net.Options
{
    public class CheckLicensingOptions
    {
        private static CheckLicensingOptions s_default;

        internal static CheckLicensingOptions Default =>
            s_default ?? (s_default = new CheckLicensingOptions());
    }

    public class PartnerLoginOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceModel { get; set; }
        public string Version { get; set; }

        public bool? IncludeUrls { get; set; }
        public bool? ReturnDeviceType { get; set; }
        public bool? ReturnUpdatePromptVersions { get; set; }
    }
}
