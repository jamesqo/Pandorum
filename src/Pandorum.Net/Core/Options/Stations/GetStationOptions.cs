﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Core.Options.Stations
{
    public class GetStationOptions
    {
        public string StationToken { get; set; }
        public bool IncludeExtendedAttributes { get; set; }
    }
}
