using Pandorum.Core.DataTransfer.Tracks;
using Pandorum.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Tracks
{
    public class Track : ITrack
    {
        private readonly string _trackToken;

        internal Track(TrackDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _trackToken = dto.TrackToken;

            Urls = new TrackUrls(dto);
            Station = new TokenStation(dto.StationId);
        }

        // TODO: Expose songRating, other properties

        public TrackUrls Urls { get; }
        public IStation Station { get; }
        public AudioFileCollection AudioFiles { get; }

        string ITrack.TrackToken => _trackToken;
    }
}
