using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations.Core
{
    internal static class PandoraClientWrapperExtensions
    {
        public static IPandoraJsonClient JsonClient(this IPandoraClientWrapper wrapper)
        {
            return wrapper.InnerClient.JsonClient;
        }
    }
}
