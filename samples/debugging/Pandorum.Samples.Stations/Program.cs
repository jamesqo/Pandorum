using Pandorum.Net;
using Pandorum.Samples.Helpers;
using Pandorum.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.Stations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AsyncHelpers.RunSynchronously(async () =>
            {
                Console.WriteLine("Logging in to Pandora...");

                using (var client = await PandoraHelpers.Login())
                {
                    Console.WriteLine("Finished logging in.");
                    client.Settings.Endpoint = PandoraEndpoints.Tuner.HttpUri; // Some things only seem to work with HTTP

                    Console.WriteLine("The checksum of your station list is:");
                    var original = await client.Stations.Checksum();
                    Console.WriteLine(original);

                    Console.WriteLine("Here it is again, calling user.getStationList:");
                    var reference = new ChecksumReference();
                    await client.Stations.List(reference);
                    Console.WriteLine(reference);

                    Console.WriteLine($"Have any of your stations been modified between API calls? {original != reference.Checksum}");

                    Console.WriteLine("List of stations:");
                    var list = await client.Stations.List();

                    int index = 0;
                    foreach (var station in list)
                    {
                        Console.WriteLine($"Station #{index + 1}");
                        Console.WriteLine($"Name: {station.Name}");
                        Console.WriteLine($"Date created: {station.DateCreated}");
                        index++;
                    }

                    string query = Environment.GetEnvironmentVariable("PANDORUM_QUERY") ?? "The Weeknd";
                    Console.WriteLine($"Searching for {query}...");
                    var results = await client.Stations.Search(query);

                    Console.WriteLine("Artists:");
                    foreach (var artist in results.Artists)
                    {
                        Console.WriteLine($"Name: {artist.Name}");
                    }

                    Console.WriteLine("Songs:");
                    foreach (var song in results.Songs)
                    {
                        Console.WriteLine($"Name: {song.Name}, Artist: {song.ArtistName}");
                    }
                    
                    Console.WriteLine("Genres:");
                    foreach (var genre in results.Genres)
                    {
                        Console.WriteLine($"Name: {genre.Name}");
                    }
                }
            });
        }
    }
}
