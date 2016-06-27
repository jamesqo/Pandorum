using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    internal interface IStationInfo
    {
        string Name { get; }
        DateTimeOffset DateCreated { get; }
        
        string DetailUrl { get; }
        string SharingUrl { get; }

        bool IsShared { get; }
        bool IsRenameable { get; }
        bool IsDeletable { get; }
        bool IsQuickMix { get; }
        bool CanAddMusic { get; }
        bool SuppressesVideoAds { get; }
        bool HasEditableDescription { get; }
        bool RequiresCleanAds { get; }
        
        IEnumerable<string> Tags { get; } // "genre": [...]

        // TODO: QuickMix?
    }
}
