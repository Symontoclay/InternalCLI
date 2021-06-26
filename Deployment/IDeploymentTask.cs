using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    public interface IDeploymentTask: IObjectToString
    {
        bool? IsValid { get; }
        IReadOnlyList<string> ValidationMessages { get; }
        void ValidateOptions();
        void Run();
    }
}
