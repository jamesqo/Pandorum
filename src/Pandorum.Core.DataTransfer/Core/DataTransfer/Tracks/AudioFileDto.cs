using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Tracks
{
    public class AudioFileDto
    {
        // TODO: For some reason, this is sent as a string (e.g. "64").
        // Not a problem since Json.NET parses it, but figure out why.
        // Maybe it should be a string, and we call int.{Try}Parse manually?
        public int Bitrate { get; set; }
        public string Encoding { get; set; }
        public string AudioUrl { get; set; }
        public string Protocol { get; set; } // should be "http"
    }
}
