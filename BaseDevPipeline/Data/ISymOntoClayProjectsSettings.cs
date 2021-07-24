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
        string SecretFilePath { get; }
        string GetSecret(string key);
        Dictionary<string, string> GetSecrets();
        IReadOnlyList<IUtityExeInstance> UtityExeInstances { get; }
        IReadOnlyList<ISolutionSettings> Solutions { get; }
        IReadOnlyList<IProjectSettings> Projects { get; }
        IReadOnlyList<IArtifactSettings> Artifacts { get; }
        IReadOnlyList<ILicenseSettings> Licenses { get; }

        ISolutionSettings GetSolution(KindOfProject kind);
        IReadOnlyList<ISolutionSettings> GetSolutions(KindOfProject kind);

        IProjectSettings GetProject(KindOfProject kind);
        IReadOnlyList<IProjectSettings> GetProjects(KindOfProject kind);

        IArtifactSettings GetArtifact(KindOfArtifact kind);
        IReadOnlyList<IArtifactSettings> GetArtifacts(KindOfArtifact kind);

        ILicenseSettings GetLicense(string name);
    }
}
