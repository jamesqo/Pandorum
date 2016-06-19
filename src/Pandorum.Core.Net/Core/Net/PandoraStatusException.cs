using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    public class PandoraStatusException : Exception
    {
        public PandoraStatusException(int code, string message)
            : base(message)
        {
            Code = code;
        }

        public int Code { get; }
    }
}
