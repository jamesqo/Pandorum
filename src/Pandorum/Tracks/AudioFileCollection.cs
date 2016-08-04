using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Tracks
{
    public class AudioFileCollection : IEnumerable<AudioFile>
    {
        public AudioFile HighQuality { get; }
        public AudioFile MediumQuality { get; }
        public AudioFile LowQuality { get; }

        public IEnumerable<AudioFile> Additional { get; }

        public IEnumerator<AudioFile> GetEnumerator()
        {
            yield return HighQuality;
            yield return MediumQuality;
            yield return LowQuality;

            foreach (var file in Additional)
            {
                yield return file;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
