using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum
{
    public static class PandoraClientExtensions
    {
        public static T Http<T>(this T client) where T : IPandoraClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            ChangeEndpointScheme(client.Inner.Settings, "http");
            return client;
        }

        public static T Https<T>(this T client) where T : IPandoraClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            ChangeEndpointScheme(client.Inner.Settings, "https");
            return client;
        }

        private static void ChangeEndpointScheme(PandoraClientSettings settings, string scheme)
        {
            settings.Endpoint = UriHelpers.ChangeScheme(settings.Endpoint, scheme);
        }

        // Convenience method
        [DebuggerStepThrough]
        internal static IPandoraJsonClient JsonClient(this IPandoraClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return client.Inner.JsonClient;
        }
    }
}
