using Pandorum.Samples.Helpers;
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

                string username = PandoraHelpers.Username;
                string password = PandoraHelpers.Password;
                await client.UserLogin(username, password);
            }
        }
    }
}
