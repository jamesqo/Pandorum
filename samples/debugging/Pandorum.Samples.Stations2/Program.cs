using Pandorum.Net;
using Pandorum.Samples.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

                    Console.WriteLine("Getting expanded info...");
                    var expanded = await client.Stations.ExpandInfo(created);

                    WriteProperties(expanded, Console.Out);
                    WriteProperties(expanded.Feedback, Console.Out);
                    WriteProperties(expanded.Music, Console.Out);
                }
                finally
                {
                    Console.WriteLine("Deleting the newly created station...");
                    await client.Stations.Delete(created);
                    Console.WriteLine("Successfully deleted!");
                }
            });
        }

        private static void WriteProperties(object obj, TextWriter dest)
        {
            foreach (var property in obj.GetType().GetRuntimeProperties())
            {
                dest.WriteLine($"{property.Name} = {property.GetValue(obj)}");
            }
        }
    }
}
