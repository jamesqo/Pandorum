using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public static class StationsClientExtensions
    {
        // Roslyn picks up on the IAddableSeed-based method if we attempt this
        // Task<ExpandedArtist> AddSeed(this StationsClient, IStation, Artist)

        public async static Task<ExpandedArtist> AddArtist(this StationsClient client, IStation station, Artist artist)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            // The client should validate the rest of the arguments

            var result = await client.AddSeed(station, artist).ConfigureAwait(false);
            return (ExpandedArtist)result;
        }

        // TODO: AddGenreStation

        public async static Task<ExpandedSong> AddSong(this StationsClient client, IStation station, Song song)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var result = await client.AddSeed(station, song).ConfigureAwait(false);
            return (ExpandedSong)result;
        }
    }
}
