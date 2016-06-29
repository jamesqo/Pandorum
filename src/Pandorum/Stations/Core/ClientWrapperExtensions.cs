using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations.Core
{
    internal static class ClientWrapperExtensions
    {
        [DebuggerStepThrough]
        public static IPandoraJsonClient JsonClient(this IClientWrapper wrapper)
        {
            if (wrapper == null)
                throw new ArgumentNullException(nameof(wrapper));

            return wrapper.InnerClient.JsonClient;
        }
    }
}
