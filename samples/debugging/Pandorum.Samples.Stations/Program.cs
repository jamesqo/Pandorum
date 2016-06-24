using Pandorum.Net;
using Pandorum.Samples.Helpers;
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
                    Console.WriteLine(await client.Stations.Checksum());

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
                }
            });
        }
    }
}
