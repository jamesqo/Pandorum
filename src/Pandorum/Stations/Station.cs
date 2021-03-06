﻿using Newtonsoft.Json;
using Pandorum.Core;
using Pandorum.Core.DataTransfer;
using Pandorum.Core.DataTransfer.Stations;
using Pandorum.Stations.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Station : IStation, IStationInfo
    {
        private readonly string _token;
        private readonly QuickMixStationInfo _quickMix;

        internal Station(StationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Name = dto.StationName;
            DateCreated = dto.DateCreated.ToDateTimeOffset();
            IsShared = dto.IsShared;
            DetailUrl = dto.StationDetailUrl;
            SharingUrl = dto.StationSharingUrl;
            IsRenameable = dto.AllowRename;
            IsDeletable = dto.AllowDelete;
            CanAddMusic = dto.AllowAddMusic;
            SuppressesVideoAds = dto.SuppressVideoAds;
            HasEditableDescription = dto.AllowEditDescription;
            Tags = dto.Genre?.AsReadOnly().AsEnumerable() ?? ImmutableCache.EmptyArray<string>();
            IsQuickMix = dto.IsQuickMix;
            RequiresCleanAds = dto.RequiresCleanAds;
            _token = dto.StationToken;

            var stations = dto.QuickMixStationIds?.Select(id => new TokenStation(id)); // Select is lazy, so even if QuickMixStationsId isn't null this will only make 1 allocation
            _quickMix = new QuickMixStationInfo(stations ?? ImmutableCache.EmptyArray<TokenStation>()); // this is a struct, so we eagerly allocate
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
        public IEnumerable<string> Tags { get; }
        public bool IsQuickMix { get; }
        public bool RequiresCleanAds { get; }

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

        public override string ToString() => Name;
    }
}
