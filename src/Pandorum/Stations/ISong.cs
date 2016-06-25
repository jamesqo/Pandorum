using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    internal interface ISong
    {
        string Name { get; }
        string ArtistName { get; }
    }
}
