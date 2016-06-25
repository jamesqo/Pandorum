using Newtonsoft.Json;
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
    public class Station
    {
        private readonly QuickMixStationInfo _quickMix;

        internal Station(StationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.StationName;
            Token = dto.StationToken;
            DateCreated = dto.DateCreated.ToDateTimeOffset();
            IsShared = dto.IsShared;
            DetailUrl = dto.StationDetailUrl;
            SharingUrl = dto.StationSharingUrl;
            IsRenameable = dto.AllowRename;
            IsDeletable = dto.AllowDelete;
            CanAddMusic = dto.AllowAddMusic;
            SuppressesVideoAds = dto.SuppressVideoAds;
            HasEditableDescription = dto.AllowEditDescription;
            Genres = dto.Genre?.AsReadOnly().AsEnumerable() ?? ImmutableCache.EmptyArray<string>();
            IsQuickMix = dto.IsQuickMix;
            RequiresCleanAds = dto.RequiresCleanAds;
            _quickMix = new QuickMixStationInfo(dto.QuickMixStationIds); // this is a struct, so we eagerly allocate
        }

        public string Name { get; }
        public DateTimeOffset DateCreated { get; }
        public bool IsShared { get; }
        public string DetailUrl { get; }
        public string SharingUrl { get; }
        public bool IsRenameable { get; }
        public bool IsDeletable { get; }
        public bool CanAddMusic { get; }
        public bool SuppressesVideoAds { get; }
        public bool HasEditableDescription { get; }
        public IEnumerable<string> Genres { get; }
        public bool IsQuickMix { get; }
        public bool RequiresCleanAds { get; }

        public QuickMixStationInfo QuickMix
        {
            get
            {
                if (!IsQuickMix)
                    throw new InvalidOperationException($"You cannot call {nameof(QuickMix)} on a station that isn't a QuickMix station.");

                Debug.Assert(_quickMix.StationIds != null);
                return _quickMix;
            }
        }

        internal string Token { get; }

        public override string ToString() => Name;
    }
}
