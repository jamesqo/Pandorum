using Pandorum.Core;
using Pandorum.Core.Options.Stations;
using Pandorum.Stations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class FeedbackClient : IPandoraClient
    {
        private readonly PandoraClient _inner;

        internal FeedbackClient(PandoraClient inner)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));

            _inner = inner;
        }

        PandoraClient IPandoraClient.Inner => _inner;

        public async Task Remove(IRating rating)
        {
            var options = CreateRemoveOptions(rating);
            await this.JsonClient().DeleteFeedback(options).ConfigureAwait(false);
        }

        private static DeleteFeedbackOptions CreateRemoveOptions(IRating rating)
        {
            return new DeleteFeedbackOptions { FeedbackId = rating.FeedbackId };
        }
    }
}
