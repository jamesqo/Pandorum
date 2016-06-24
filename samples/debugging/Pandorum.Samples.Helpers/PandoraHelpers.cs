using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.Helpers
{
    public static class PandoraHelpers
    {
        private static string s_username;
        private static string s_password;

        public static string Username =>
            s_username ?? (s_username = Environment.GetEnvironmentVariable("PANDORUM_USERNAME"));

        public static string Password =>
            s_password ?? (s_password = Environment.GetEnvironmentVariable("PANDORUM_PASSWORD"));

        public async static Task<PandoraClient> Login()
        {
            var client = new PandoraClient();

            try
            {
                await client.Login(Username, Password);
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }
    }
}
