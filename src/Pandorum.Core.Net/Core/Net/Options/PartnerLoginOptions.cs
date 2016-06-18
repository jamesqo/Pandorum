using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net.Options
{
    public class PartnerLoginOptions
    {
        private OptionalBool _includeUrls;
        private OptionalBool _returnDeviceType;
        private OptionalBool _returnUpdatePromptVersions;

        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceModel { get; set; }
        public string Version { get; set; }

        public bool? IncludeUrls
        {
            get { return _includeUrls; }
            set { _includeUrls = value; }
        }
        public bool? ReturnDeviceType
        {
            get { return _returnDeviceType; }
            set { _returnDeviceType = value; }
        }
        public bool? ReturnUpdatePromptVersions
        {
            get { return _returnUpdatePromptVersions; }
            set { _returnUpdatePromptVersions = value; }
        }
    }
}
