﻿using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSandBox.RestoredDeploymentTasks.Serialization;

namespace TestSandBox.RestoredDeploymentTasks
{
    public interface INewDeploymentTask : IObjectToString
    {
        void SetParentTask(INewDeploymentTask parentTask);
        bool? IsValid { get; }
        IReadOnlyList<string> ValidationMessages { get; }
        void ValidateOptions();
        void Run();
        uint Deep { get; }
        uint NextDeep { get; }
        string Key { get; }
        NewDeploymentTaskRunInfo GetChildDeploymentTaskRunInfo(string key);
        bool ContainsChild(string key);
        void AddChildDeploymentTaskRunInfo(NewDeploymentTaskRunInfo item);
    }
}
