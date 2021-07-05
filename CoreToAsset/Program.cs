﻿using Deployment.DevPipelines.CoreToAsset;
using System;

namespace CoreToAsset
{
    class Program
    {
        static void Main(string[] args)
        {
            var coreToAssetTask = new CoreToAssetDevPipeline();
            coreToAssetTask.Run();
        }
    }
}
