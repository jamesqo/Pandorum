using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Net
{
    public class PandoraStatusException : Exception
    {
        public PandoraStatusException() { }
        public PandoraStatusException(string message) : base(message) { }
        public PandoraStatusException(string message, Exception inner) : base(message, inner) { }
    }
}
