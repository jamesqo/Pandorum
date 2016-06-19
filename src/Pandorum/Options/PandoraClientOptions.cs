using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Options
{
    public class PandoraClientOptions
    {
        private static PandoraClientOptions s_default;

        internal static PandoraClientOptions Default =>
            s_default ?? (s_default = new PandoraClientOptions());
    }
}
