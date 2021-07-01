using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data
{
    public interface IProjectSettings : IObjectToString
    {
        ISolutionSettings Solution { get; }
        KindOfProjectSource Kind { get; }
        string Path { get; }
        string CsProjPath { get; }
        string LicenseName { get; }
        ILicenseSettings License { get; }
    }
}
