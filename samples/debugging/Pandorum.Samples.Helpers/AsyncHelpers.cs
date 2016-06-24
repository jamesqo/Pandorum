using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Samples.Helpers
{
    public static class AsyncHelpers
    {
        public static void RunSynchronously(Func<Task> func)
        {
            func().GetAwaiter().GetResult();
        }
    }
}
