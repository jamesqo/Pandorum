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

                if (Environment.GetEnvironmentVariable("PANDORUM_GENRE_TEST") != "false")
                {
                    Console.WriteLine("Testing genreStations API...");

                    var checksum = await client.Stations.Genres.Checksum();
                    Console.WriteLine($"Checksum: {checksum}");

                    var list = await client.Stations.Genres.List();
                    // TODO: Category should implement IGrouping
                    // Return type may want to implement ILookup
                    foreach (var category in list)
                    {
                        Console.WriteLine(category.Name);
                        foreach (var station in category)
                        {
                            Console.WriteLine(station);
                        }
                    }
                }

                // TODO: Add conditional here
                {
                    Console.WriteLine("Creating new test station...");
                    var query = Environment.GetEnvironmentVariable("PANDORUM_GENRE_QUERY") ?? "Pop";

                    var results = await client.Stations.Search(query);
                    Console.WriteLine("Finished searching.");

                    var genre = results.Genres.First();
                    Console.WriteLine($"Calling createStation with genre {genre}...");

                    var station = await client.Stations.Create(genre);

                    try
                    {
                        Console.WriteLine("Adding another artist seed...");
                        var query2 = Environment.GetEnvironmentVariable("PANDORUM_ARTIST_QUERY") ?? "Jason Derulo";
                        Console.WriteLine($"Searching for {query2}");

                        var results2 = await client.Stations.Search(query2);
                        var artist = results2.Artists.First();

                        Console.WriteLine($"Adding {artist} to the mix...");
                        var removable = await client.Stations.AddSeed(station, artist);

                        Console.WriteLine("Adding a song seed...");
                        var query3 = Environment.GetEnvironmentVariable("PANDORUM_SONG_QUERY") ?? "Counting Stars";
                        Console.WriteLine($"Searching for: {query3}");

                        var results3 = await client.Stations.Search(query3);
                        var song = results3.Songs.FirstOrDefault(s => s.ArtistName == "One Republic") ?? results3.Songs.First();
                        Console.WriteLine($"Adding seed {song} to the station...");

                        var removable2 = await client.Stations.AddSeed(station, song);
                        Console.WriteLine("Finished.");
                        // Console.WriteLine($"artUrl of song: {removable2.ArtUrl}");
                        // Console.WriteLine($"dateCreated: {removable2.DateCreated}");

                        Console.WriteLine("Finally! Getting extended info...");
                        var expanded = await client.Stations.ExpandInfo(station);
                        Debug.Assert(expanded.GetType() == typeof(ExpandedStation)); // should not subclass

                        foreach (var property in typeof(ExpandedStation).GetRuntimeProperties())
                        {
                            try
                            {
                                Console.WriteLine($"{property} = {property.GetValue(expanded)}");
                            }
                            catch { }
                        }

                        var removable3 = expanded.Music.Genres.First();
                        Console.WriteLine($"Removing genre {removable3}...");
                        await client.Stations.RemoveSeed(removable3);
                        Console.WriteLine("Finished!");

                        Console.WriteLine("Re-retrieving extended info to reflect the results...");
                        expanded = await client.Stations.ExpandInfo(expanded);

                        foreach (var property in expanded.GetType().GetRuntimeProperties())
                        {
                            try
                            {
                                Console.WriteLine($"{property} = {property.GetValue(expanded)}");
                            }
                            catch { }
                        }
                    }
                    finally
                    {
                        Console.WriteLine("Deleting the test station...");
                        await client.Stations.Delete(station);
                    }
                }
            });
        }
    }
}
