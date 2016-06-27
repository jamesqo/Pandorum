using Pandorum.Net;
using Pandorum.Samples.Helpers;
using Pandorum.Stations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pandorum.Samples.Stations3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PandoraHelpers.Session(async client =>
            {
                client.Settings.Endpoint = PandoraEndpoints.Tuner.HttpUri;

                Console.WriteLine("Creating new test station...");
                var query = Environment.GetEnvironmentVariable("PANDORUM_ARTIST_QUERY") ?? "Jason Derulo";

                var results = await client.Stations.Search(query);
                Console.WriteLine("Finished searching.");

                var artist = results.Artists.First();
                Console.WriteLine($"Calling createStation with artist {artist}...");

                var station = await client.Stations.Create(artist);

                try
                {
                    Console.WriteLine("Adding another artist seed...");
                    var query2 = Environment.GetEnvironmentVariable("PANDORUM_ARTIST_QUERY_2") ?? "Taylor Swift";
                    Console.WriteLine($"Searching for {query2}");

                    var results2 = await client.Stations.Search(query2);
                    var artist2 = results2.Artists.First();

                    Console.WriteLine($"Adding {artist2} to the mix...");
                    var removable = await client.Stations.AddSeed(station, artist2);

                    Console.WriteLine("Adding a song seed...");
                    var songQuery = Environment.GetEnvironmentVariable("PANDORUM_SONG_QUERY") ?? "Counting Stars";
                    Console.WriteLine($"Searching for: {songQuery}");

                    var songResults = await client.Stations.Search(songQuery);
                    var song = songResults.Songs.FirstOrDefault(s => s.ArtistName == "One Republic") ?? songResults.Songs.First();
                    Console.WriteLine($"Adding seed {song} to the station...");

                    var removable2 = await client.Stations.AddSeed(station, song);
                    Console.WriteLine("Finished.");
                    // Console.WriteLine($"artUrl of song: {expandedSong.ArtUrl}");
                    // Console.WriteLine($"dateCreated: {expandedSong.DateCreated}");

                    Console.WriteLine("Adding a genre station to the mix...");
                    var query3 = Environment.GetEnvironmentVariable("PANDORUM_GENRE_QUERY") ?? "Pop";
                    Console.WriteLine($"Searching for: {query3}");

                    var results3 = await client.Stations.Search(query3);
                    var genreStation = results3.GenreStations.First();

                    Console.WriteLine($"Adding genre station: {genreStation}");
                    var removable3 = await client.Stations.AddSeed(station, genreStation);
                    // var expanded = await client.Stations.AddGenreStation(station, genreStation);
                    // Console.WriteLine(...);

                    Console.WriteLine("Finally! Getting extended info...");
                    var expanded2 = await client.Stations.ExpandInfo(station);
                    Debug.Assert(expanded2.GetType() == typeof(ExpandedStation)); // should not subclass

                    foreach (var property in typeof(ExpandedStation).GetRuntimeProperties())
                    {
                        Console.WriteLine($"{property} = {property.GetValue(expanded2)}");
                    }
                }
                finally
                {
                    Console.WriteLine("Deleting the test station...");
                    await client.Stations.Delete(station);
                }
            });
        }
    }
}
