using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class ExpandedArtist : IArtist, IExpandedSeed
    {
        internal ExpandedArtist(ExpandedArtistDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // TODO
        }

        public string Name { get; }

        public ISeedRemover Remover { get; }
    }
}
