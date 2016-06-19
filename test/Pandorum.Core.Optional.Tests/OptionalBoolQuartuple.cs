using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Optional.Tests
{
    // This is necessary so we can take advantage of implicit conversions in our tests

    public class OptionalBoolQuartuple : Tuple<OptionalBool, OptionalBool, OptionalBool, OptionalBool>
    {
        public OptionalBoolQuartuple(OptionalBool item1, OptionalBool item2, OptionalBool item3, OptionalBool item4)
            : base(item1, item2, item3, item4)
        {
        }
    }
}
