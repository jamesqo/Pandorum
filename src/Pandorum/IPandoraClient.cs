﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum
{
    public interface IPandoraClient
    {
        PandoraClient Inner { get; }
    }
}
