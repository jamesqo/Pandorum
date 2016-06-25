using Pandorum.Net;
using Pandorum.Samples.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.Stations2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Logging in...");
            
            PandoraHelpers.Login(async client =>
            {
                client.Settings.Endpoint = PandoraEndpoints.Tuner.HttpUri;

                Console.WriteLine("Finished logging in.");

                var query = Environment.GetEnvironmentVariable("PANDORUM_QUERY") ?? "Taylor Swift";
                Console.WriteLine($"Searching for {query}...");

                var results = await client.Stations.Search(query);
                var seed = results.Artists.First();

                Console.WriteLine("Creating a new station...");
                Console.WriteLine($"Using seed: {seed.GetType()}, {seed}");

                await client.Stations.Create(seed);
            });
        }
    }
}
