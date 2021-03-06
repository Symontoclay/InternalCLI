﻿using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data
{
    public interface IUtityExeInstance : IObjectToString
    {
        string Version { get; }
        string Path { get; }
    }
}
