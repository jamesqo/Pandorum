using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using static Pandorum.Tracks.AudioEncoding;

namespace Pandorum.Tracks
{
    public struct AudioFormat
    {
        public AudioFormat(AudioEncoding encoding, int bitrate, Protocol protocol)
        {
            // We leave these checks out in Release because they're costly
            // at runtime
            // We could do something like
            // if (encoding < AudioEncoding.Min || encoding > AudioEncoding.Max)
            // but then we'd have remember to update this type everytime a new
            // encoding was added.
            // Maybe AudioEncoding should actually define Min/Max, and it'd
            // be AudioEncoding's job to update it whenever a new value was
            // added?
            Debug.Assert(Enum.IsDefined(typeof(AudioEncoding), encoding));
            Debug.Assert(Enum.IsDefined(typeof(Protocol), protocol));

            // TODO: Maybe a bitrate of -1 should be allowed, to express
            // unkown bitrate?
            // Should 0 be allowed?
            if (bitrate <= 0)
                throw new ArgumentOutOfRangeException(nameof(bitrate));

            Encoding = encoding;
            Bitrate = bitrate;
            Protocol = protocol;
        }

        public int Bitrate { get; }
        public AudioEncoding Encoding { get; }
        public Protocol Protocol { get; }

        public override string ToString()
        {
            // Explicitly call Bitrate.ToString()
            // and use "_" rather than '_' here,
            // since we want to call the string-
            // based overload and avoid boxing

            return string.Concat(
                ProtocolString, "_",
                Bitrate.ToString(), "_",
                EncodingString);
        }

        private string ProtocolString
        {
            get
            {
                Debug.Assert(Protocol == Protocol.Http); // that's the only available protocol for now
                return "HTTP";
            }
        }

        private string EncodingString
        {
            get
            {
                switch (Encoding)
                {
                    case AacMono:
                        return "AAC_MONO";
                    case Aac:
                        return "AAC";
                    case AacPlus:
                        return "AACPLUS";
                    case AacPlusAdts:
                        return "AACPLUS_ADTS";
                    case Mp3:
                        return "MP3";
                    case Wma:
                        return "WMA";
                }

                Debug.Assert(false, $"{nameof(Encoding)} should be one of the values in {nameof(AudioEncoding)}.");
                return default(string);
            }
        }
    }
}
