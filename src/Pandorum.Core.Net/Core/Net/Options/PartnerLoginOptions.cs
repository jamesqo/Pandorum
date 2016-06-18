using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net.Options
{
    public class PartnerLoginOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceModel { get; set; }
        public string Version { get; set; }

        public OptionalBool IncludeUrls { get; set; }
        public OptionalBool ReturnDeviceType { get; set; }
        public OptionalBool ReturnUpdatePromptVersions { get; set; }
    }
}
