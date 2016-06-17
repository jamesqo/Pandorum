using Newtonsoft.Json.Linq;
using Pandorum.Core;
using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pandorum
{
    public class PandoraClient : IDisposable
    {
        private IPandoraJsonClient _baseClient;

        public PandoraClient()
            : this(useHttps: true) // TODO: Which one is better? HTTP or HTTPS?
        {
        }

        public PandoraClient(bool useHttps)
            // TODO: Which one is better? Internal or non-internal?
            : this(useHttps ?
                  PandoraEndpoints.TunerHttps :
                  PandoraEndpoints.TunerHttp)
        {
        }

        public PandoraClient(string endpoint)
            : this(new VerifyingJsonClient(endpoint))
        {
        }

        private PandoraClient(IPandoraJsonClient baseClient)
        {
            _baseClient = baseClient;
        }

        // API functionality

        public async Task<bool> CheckLicensing()
        {
            var response = await _baseClient.CheckLicensing().ConfigureAwait(false);
            return (bool)response["result"]["isAllowed"];
        }
        
        // Dispose logic

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_baseClient != null)
                {
                    _baseClient.Dispose();
                    _baseClient = null;
                }
            }
        }
    }
}
