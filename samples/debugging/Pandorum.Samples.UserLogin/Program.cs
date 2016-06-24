using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.UserLogin
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

                string username = Environment.GetEnvironmentVariable("PANDORUM_USERNAME");
                string password = Environment.GetEnvironmentVariable("PANDORUM_PASSWORD");
                await client.UserLogin(username, password);
            }
        }
    }
}
