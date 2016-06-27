using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    internal class TokenStation : IStation
    {
        public TokenStation(string token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            Token = token;
        }

        public string Token { get; }
    }
}
