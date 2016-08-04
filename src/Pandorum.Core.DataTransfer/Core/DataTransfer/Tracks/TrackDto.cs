using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Tracks
{
    public class TrackDto
    {
        public string TrackToken { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string AmazonAlbumUrl { get; set; }
        public string SongExplorerUrl { get; set; }
        public string AlbumArtUrl { get; set; }
        public string ArtistDetailUrl { get; set; }
        public AudioFilesDto AudioUrlMap { get; set; }
        public string iTunesSongUrl { get; set; }
        public string[] AdditionalAudioUrl { get; set; }
        public string AmazonAlbumAsin { get; set; }
        public string AmazonAlbumDigitalAsin { get; set; }
        public string ArtistExplorerUrl { get; set; }
        public string SongName { get; set; }
        public string AlbumDetailUrl { get; set; }
        public string SongDetailUrl { get; set; }
        public string StationId { get; set; }
        public int SongRating { get; set; }
        public string TrackGain { get; set; } // TODO: Maybe this should be a double. I don't know what it represents.
        public string AlbumExplorerUrl { get; set; }
        public bool AllowFeedback { get; set; }
        public string AmazonSongDigitalAsin { get; set; }
        public string NowPlayingStationAdUrl { get; set; }
        // TODO: AdToken?
    }
}
