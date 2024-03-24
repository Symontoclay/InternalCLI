using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public interface INewDeploymentTask : IObjectToString
    {
        bool? IsValid { get; }
        IReadOnlyList<string> ValidationMessages { get; }
        void ValidateOptions();
        void Run();
        uint Deep { get; }
        uint NextDeep { get; }
    }
}
