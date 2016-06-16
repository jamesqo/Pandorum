using Pandorum.Core.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    // NOTE: It is important that this type is a struct,
    // otherwise the semantics of the With* methods will
    // not work.
    public struct PandoraUriBuilder
    {
        public string Endpoint { get; private set; }
        public string Method { get; private set; }
        public string AuthToken { get; private set; }
        public string PartnerId { get; private set; }
        public string UserId { get; private set; }

        public PandoraUriBuilder WithEndpoint(string endpoint)
        {
            var copy = this;
            copy.Endpoint = endpoint;
            return copy;
        }

        public PandoraUriBuilder WithMethod(string method)
        {
            var copy = this;
            copy.Method = method;
            return copy;
        }

        public PandoraUriBuilder WithAuthToken(string authToken)
        {
            var copy = this;
            copy.AuthToken = authToken;
            return copy;
        }

        public PandoraUriBuilder WithPartnerId(string partnerId)
        {
            var copy = this;
            copy.PartnerId = partnerId;
            return copy;
        }

        public PandoraUriBuilder WithUserId(string userId)
        {
            var copy = this;
            copy.UserId = userId;
            return copy;
        }

        public override string ToString()
        {
            var sb = StringBuilderPool.Default.Borrow();
            try
            {
                sb.Append(Endpoint);

                var qb = new QueryBuilder(sb);
                qb.Add("method", Method);
                qb.Add("auth_token", AuthToken);
                qb.Add("partner_id", PartnerId);
                qb.Add("user_id", UserId);
                return qb.ToString();
            }
            finally
            {
                StringBuilderPool.Default.Return(sb);
            }
        }
    }
}
