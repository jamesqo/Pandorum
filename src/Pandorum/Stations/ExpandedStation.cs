using Pandorum.Core;
using Pandorum.Core.DataTransfer;
using Pandorum.Core.DataTransfer.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class ExpandedStation : IStation, IStationInfo
    {
        private readonly string _token;
        private readonly QuickMixStationInfo _quickMix;

        internal ExpandedStation(ExpandedStationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Debug.Assert(dto.StationId == dto.StationToken); // documented to be the same

            Name = dto.StationName;
            DateCreated = dto.DateCreated.ToDateTimeOffset();
            DetailUrl = dto.StationDetailUrl;
            SharingUrl = dto.StationSharingUrl;
            IsShared = dto.IsShared;
            IsRenameable = dto.AllowRename;
            IsDeletable = dto.AllowDelete;
            IsQuickMix = dto.IsQuickMix;
            CanAddMusic = dto.AllowAddMusic;
            SuppressesVideoAds = dto.SuppressVideoAds;
            HasEditableDescription = dto.AllowEditDescription;
            RequiresCleanAds = dto.RequiresCleanAds;
            Tags = dto.Genre?.AsReadOnly().AsEnumerable() ?? ImmutableCache.EmptyArray<string>();

            _token = dto.StationToken;

            ArtUrl = dto.ArtUrl;
            Music = dto.IsQuickMix ? null : new StationSeeds(dto.Music); // "music" will not be present for quickMix stations
            // TODO: Find a more appropriate way to handle this than setting Music to null
            Feedback = new Feedback(dto.Feedback);

            var stations = dto.QuickMixStationIds?.Select(id => new TokenStation(id));
            _quickMix = new QuickMixStationInfo(stations ?? ImmutableCache.EmptyArray<TokenStation>());
        }

        public string Name { get; }
        public DateTimeOffset DateCreated { get; }
        public string DetailUrl { get; }
        public string SharingUrl { get; }
        public bool IsShared { get; }
        public bool IsRenameable { get; }
        public bool IsDeletable { get; }
        public bool IsQuickMix { get; }
        public bool CanAddMusic { get; }
        public bool SuppressesVideoAds { get; }
        public bool HasEditableDescription { get; }
        public bool RequiresCleanAds { get; }
        public IEnumerable<string> Tags { get; }

        public string ArtUrl { get; }
        public StationSeeds Music { get; }
        public Feedback Feedback { get; }

        public QuickMixStationInfo QuickMix
        {
            get
            {
                Debug.Assert(_quickMix.Stations != null);

                if (!IsQuickMix)
                    throw new InvalidOperationException($"You cannot call {nameof(QuickMix)} on a station that isn't a QuickMix station.");

                return _quickMix;
            }
        }

        string IStation.Token => _token;
    }
}
