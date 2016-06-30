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
        // If/when Uri.UriSchemeHttp{s} is exposed in
        // .NET Core, we may want to get rid of these
        // and add a `using static System.Uri` at the top
        private const string UriSchemeHttp = "http";
        private const string UriSchemeHttps = "https";

        public static T Http<T>(this T client) where T : IPandoraClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            ChangeEndpointScheme(client.Inner.Settings, UriSchemeHttp);
            return client;
        }

        // NOTE: The APIs that accept an Action/Func are experimental (same for Https)

        public static void Http<T>(this T client, Action<T> action) where T : IPandoraClient
        {
            RunWithScheme(client, action, UriSchemeHttp);
        }

        public static TResult Http<T, TResult>(this T client, Func<T, TResult> func) where T : IPandoraClient
        {
            return RunWithScheme(client, func, UriSchemeHttp);
        }

        public static T Https<T>(this T client) where T : IPandoraClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            ChangeEndpointScheme(client.Inner.Settings, UriSchemeHttps);
            return client;
        }

        public static void Https<T>(this T client, Action<T> action) where T : IPandoraClient
        {
            RunWithScheme(client, action, UriSchemeHttps);
        }

        public static TResult Https<T, TResult>(this T client, Func<T, TResult> func) where T : IPandoraClient
        {
            return RunWithScheme(client, func, UriSchemeHttps);
        }

        // Convenience method
        [DebuggerStepThrough]
        internal static IPandoraJsonClient JsonClient(this IPandoraClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return client.Inner.JsonClient;
        }

        private static void ChangeEndpointScheme(PandoraClientSettings settings, string scheme)
        {
            settings.Endpoint = UriHelpers.ChangeScheme(settings.Endpoint, scheme);
        }

        // RunWithScheme: Run the specified Action/Func on the client after changing
        // its endpoint scheme, then set it back to what it originally was

        private static void RunWithScheme<T>(T client, Action<T> action, string scheme)
            where T : IPandoraClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var settings = client.Inner.Settings;
            var original = settings.Endpoint;

            ChangeEndpointScheme(settings, scheme);

            try
            {
                action(client);
            }
            finally
            {
                settings.Endpoint = original;
            }
        }

        private static TResult RunWithScheme<T, TResult>(T client, Func<T, TResult> func, string scheme)
            where T : IPandoraClient
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            var settings = client.Inner.Settings;
            var original = settings.Endpoint;

            ChangeEndpointScheme(settings, "http");

            try
            {
                return func(client);
            }
            finally
            {
                settings.Endpoint = original;
            }
        }
    }
}
