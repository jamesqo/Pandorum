using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class Seed
    {
        internal Seed(string musicToken)
        {
            Debug.Assert(musicToken != null);

            MusicToken = musicToken;
        }

        internal string MusicToken { get; }
    }
}
