using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data
{
    public interface ISymOntoClayProjectsSettings : IObjectToString
    {
        string BasePath { get; }
        IReadOnlyList<IUtityExeInstance> UtityExeInstances { get; }
        IReadOnlyList<ISolutionSettings> Solutions { get; }
        IReadOnlyList<IProjectSettings> Projects { get; }
        IReadOnlyList<IArtifactSettings> Artifacts { get; }
        IReadOnlyList<ILicenseSettings> Licenses { get; }
    }
}
