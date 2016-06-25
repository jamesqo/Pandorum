using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Stations
{
    public class RatingDto
    {
        public DateTimeOffsetDto DateCreated { get; set; }
        public string AlbumArtUrl { get; set; }
        public string MusicToken { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public string FeedbackId { get; set; }
        public bool IsPositive { get; set; }
    }
}
