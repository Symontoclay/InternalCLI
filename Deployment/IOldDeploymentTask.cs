using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    [Obsolete("It should be removed", true)]
    public interface IOldDeploymentTask: IObjectToString
    {
        bool? IsValid { get; }
        IReadOnlyList<string> ValidationMessages { get; }
        void ValidateOptions();
        void Run();
    }
}
