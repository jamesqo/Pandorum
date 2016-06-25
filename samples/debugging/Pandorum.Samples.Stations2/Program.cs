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

                var created = await client.Stations.Create(seed);
                Console.WriteLine($"Successfully created {created}!");

                try
                {
                    var newName = "_-_-_- Radio";
                    Console.WriteLine($"Renaming to {newName}");
                    created = await client.Stations.Rename(created, newName);
                    Console.WriteLine("Done.");
                }
                finally
                {
                    Console.WriteLine("Deleting the newly created station...");
                    await client.Stations.Delete(created);
                    Console.WriteLine("Successfully deleted!");
                }
            });
        }
    }
}
