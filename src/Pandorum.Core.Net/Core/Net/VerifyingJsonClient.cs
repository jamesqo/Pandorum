using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Pandorum.Core.Net.Options;

namespace Pandorum.Core.Net
{
    public class VerifyingJsonClient : IPandoraJsonClient
    {
        private IPandoraJsonClient _inner;

        public VerifyingJsonClient(string endpoint)
            : this(new PandoraJsonClient(endpoint))
        { }

        public VerifyingJsonClient(IPandoraJsonClient inner)
        {
            _inner = inner;
        }

        public Task<JObject> CheckLicensing()
        {
            return AwaitAndCheck(_inner.CheckLicensing());
        }

        public Task<JObject> PartnerLogin(PartnerLoginOptions options)
        {
            return AwaitAndCheck(_inner.PartnerLogin(options));
        }

        public Task<JObject> UserLogin(UserLoginOptions options)
        {
            return AwaitAndCheck(_inner.UserLogin(options));
        }

        // Dispose logic

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_inner != null)
                {
                    _inner.Dispose();
                    _inner = null;
                }
            }
        }

        // Helper methods

        private async Task<JObject> AwaitAndCheck(Task<JObject> task)
        {
            var result = await task.ConfigureAwait(false);
            CheckStatus(result);
            return result;
        }

        private void CheckStatus(JObject response)
        {
            var status = (string)response["stat"];
            if (status != "ok")
                throw new PandoraStatusException();
        }
    }
}
