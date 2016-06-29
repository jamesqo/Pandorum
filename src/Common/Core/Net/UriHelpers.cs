using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    internal static class UriHelpers
    {
        public static string ChangeScheme(string uri, string scheme)
        {
            return ChangeScheme(new Uri(uri), scheme).ToString();
        }

        public static Uri ChangeScheme(Uri uri, string scheme)
        {
            if ((object)uri == null) // use reference equality, instead of Uri.operator==
                throw new ArgumentNullException(nameof(uri));
            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            var builder = new UriBuilder(uri);
            builder.Port = -1; // Port for some reason defaults to 80
            builder.Scheme = scheme;
            return builder.Uri;
        }
    }
}
