using Pandorum.Core.Net;
using Pandorum.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum
{
    public class PandoraClientSettings
    {
        private readonly IJsonClientSettings _inner;

        public PandoraClientSettings(IJsonClientSettings inner)
        {
            _inner = inner;
        }

        public string Endpoint
        {
            get { return _inner.Endpoint; }
            set { _inner.Endpoint = value; }
        }

        public IPartnerInfo PartnerInfo
        {
            get { return _inner.PartnerInfo; }
            set { _inner.PartnerInfo = value; }
        }
    }
}
