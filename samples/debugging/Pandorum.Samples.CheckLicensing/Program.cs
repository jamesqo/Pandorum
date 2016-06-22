using Pandorum.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.CheckLicensing
{
    public class Program
    {
        public static void Main(string[] args) =>
            MainAsync(args).GetAwaiter().GetResult();

        public async static Task MainAsync(string[] args)
        {
            Console.WriteLine("Determining if Pandora is available in your country...");
            Console.WriteLine();

            // test.checkLicensing seems to only work with HTTP URIs
            string endpoint = PandoraEndpoints.Tuner.HttpUri;
            var partnerInfo = PandoraEndpoints.Tuner.iOS;

            using (var client = new PandoraClient(endpoint, partnerInfo))
            {
                Console.WriteLine(await client.CheckLicensing());
            }
        }
    }
}
