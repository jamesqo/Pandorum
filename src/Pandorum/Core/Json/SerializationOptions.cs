using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Json
{
    [Flags]
    internal enum SerializationOptions
    {
        None = 0,
        CamelCaseProperties = 1,
    }
}
