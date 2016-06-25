using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public class ChecksumReference
    {
        public string Checksum { get; set; }

        public override string ToString() => Checksum;
    }
}
