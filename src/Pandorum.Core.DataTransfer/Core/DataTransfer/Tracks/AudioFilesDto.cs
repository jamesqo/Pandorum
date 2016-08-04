using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.DataTransfer.Tracks
{
    public class AudioFilesDto
    {
        public AudioFileDto HighQuality { get; set; }
        public AudioFileDto MediumQuality { get; set; }
        public AudioFileDto LowQuality { get; set; }
    }
}
