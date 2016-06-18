using Pandorum.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum
{
    public static class PandoraEndpoints
    {
        public static class Tuner
        {
            private static IPartnerInfo s_android;
            private static IPartnerInfo s_iOS;
            private static IPartnerInfo s_palm;
            private static IPartnerInfo s_windowsMobile;

            public static IPartnerInfo Android =>
                s_android ?? (s_android = new AndroidPartnerInfo());

            public static IPartnerInfo iOS =>
                s_iOS ?? (s_iOS = new iOSPartnerInfo());

            public static IPartnerInfo Palm =>
                s_palm ?? (s_palm = new PalmPartnerInfo());

            public static IPartnerInfo WindowsMobile =>
                s_windowsMobile ?? (s_windowsMobile = new WindowsMobilePartnerInfo());

            public static string HttpUri => "http://tuner.pandora.com/services/json/";
            public static string HttpsUri => "https://tuner.pandora.com/services/json/";
        }

        public static class InternalTuner
        {
            private static IPartnerInfo s_desktop;

            public static IPartnerInfo Desktop =>
                s_desktop ?? (s_desktop = new DesktopPartnerInfo());

            public static string HttpUri => "http://internal-tuner.pandora.com/services/json/";
            public static string HttpsUri => "https://internal-tuner.pandora.com/services/json/";
        }
    }
}
