using Pandorum.Core.DataTransfer.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Tracks
{
    public class TrackUrls
    {
        internal TrackUrls(TrackDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            SongDetail = dto.SongDetailUrl;
            AlbumDetail = dto.AlbumDetailUrl;
            AlbumExplorer = dto.AlbumExplorerUrl;
            SongExplorer = dto.SongExplorerUrl;
            ArtistDetail = dto.ArtistDetailUrl;
            iTunes = dto.iTunesSongUrl;
            AlbumArt = dto.AlbumArtUrl;
            ArtistExplorer = dto.ArtistExplorerUrl;
        }

        public string SongDetail { get; }
        public string AlbumDetail { get; }
        public string AlbumExplorer { get; }
        public string SongExplorer { get; }
        public string ArtistDetail { get; }
        public string iTunes { get; }
        public string AlbumArt { get; }
        public string ArtistExplorer { get; }
        // nowPlayingStationAdUrl?
    }
}
