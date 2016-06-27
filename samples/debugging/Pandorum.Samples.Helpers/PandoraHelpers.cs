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

        public static string Username
        {
            get
            {
                if (s_username == null)
                {
                    if ((s_username = Environment.GetEnvironmentVariable("PANDORUM_USERNAME")) == null)
                    {
                        throw new InvalidOperationException("The PANDORUM_USERNAME environment variable is not set.");
                    }
                }

                return s_username;
            }
        }

        public static string Password
        {
            get
            {
                if (s_password == null)
                {
                    if ((s_password = Environment.GetEnvironmentVariable("PANDORUM_PASSWORD")) == null)
                    {
                        throw new InvalidOperationException("The PANDORUM_PASSWORD environment variable is not set.");
                    }
                }

                return s_password;
            }
        }

        public async static Task<PandoraClient> Login()
        {
            var client = new PandoraClient();

            try
            {
                await client.Login(Username, Password).ConfigureAwait(false);
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }

        // Logs in and blocks synchronously until func is completed
        public static void Session(Func<PandoraClient, Task> func)
        {
            AsyncHelpers.RunSynchronously(async () =>
            {
                using (var client = await Login().ConfigureAwait(false))
                {
                    await func(client).ConfigureAwait(false);
                }
            });
        }
    }
}
