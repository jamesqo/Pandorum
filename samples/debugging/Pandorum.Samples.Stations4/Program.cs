using Newtonsoft.Json;
using Pandorum.Net;
using Pandorum.Samples.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.Stations4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PandoraHelpers.Session(async client =>
            {
                client.Settings.Endpoint = PandoraEndpoints.Tuner.HttpUri;

                Console.WriteLine("Retrieving a list of stations...");
                var list = await client.Stations.List();
                var quickMix = list.Single(s => s.IsQuickMix);

                Console.WriteLine("Getting extended info on QuickMix...");
                var expanded = await client.Stations.ExpandInfo(quickMix);
                Console.WriteLine(JsonConvert.SerializeObject(expanded, Formatting.Indented));

                Console.WriteLine("Getting extended info on some other station...");
                var station = list.FirstOrDefault(s => s.Name == "The Hills Radio") ?? list.First(s => !s.IsQuickMix);
                expanded = await client.Stations.ExpandInfo(station);
                Console.WriteLine(JsonConvert.SerializeObject(expanded, Formatting.Indented));
            });
        }
    }
}
