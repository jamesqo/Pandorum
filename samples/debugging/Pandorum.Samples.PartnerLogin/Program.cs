using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.PartnerLogin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public async static Task MainAsync()
        {
            using (var client = new PandoraClient())
            {
                await client.PartnerLogin();

                Console.WriteLine("Succesfully logged in.");
            }
        }
    }
}
