using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations.Core
{
    internal static class ClientWrapperExtensions
    {
        public static IPandoraJsonClient JsonClient(this IClientWrapper wrapper)
        {
            return wrapper.InnerClient.JsonClient;
        }
    }
}
