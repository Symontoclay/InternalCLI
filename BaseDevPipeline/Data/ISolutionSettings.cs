using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data
{
    public interface ISolutionSettings : IObjectToString
    {
        KindOfProjectSource Kind { get; }
        string Path { get; }
        string SlnPath { get; }
        string SourcePath { get; }
        IReadOnlyList<IProjectSettings> Projects { get; }
        string LicenseName { get; }
        ILicenseSettings License { get; }
    }
}
