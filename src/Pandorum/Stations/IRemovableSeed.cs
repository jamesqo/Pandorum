﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Stations
{
    public interface IRemovableSeed : ISeed
    {
        string SeedId { get; }
    }
}
