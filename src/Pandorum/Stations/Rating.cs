using Pandorum.Core.DataTransfer;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Rating : IRating
    {
        private readonly string _feedbackId;

        internal Rating(RatingDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            
            AlbumArtUrl = dto.AlbumArtUrl;
            ArtistName = dto.ArtistName;
            SongName = dto.SongName;

            IsPositive = dto.IsPositive;
            DateCreated = dto.DateCreated.ToDateTimeOffset();

            _feedbackId = dto.FeedbackId;
        }

        // TODO: Should have a Song property that implements
        // ISongInfo and exposes ArtUrl, instead of these 3 properties,
        // once we learn how to decrypt musicToken/songIdentity
        public string SongName { get; }
        public string ArtistName { get; }
        public string AlbumArtUrl { get; }

        public bool IsPositive { get; }
        public DateTimeOffset DateCreated { get; }

        string IRating.FeedbackId => _feedbackId;
    }
}
