using SymOntoClay.Common;
using System.Collections.Generic;

namespace BaseDevPipeline.Data
{
    public interface ISymOntoClayProjectsSettings : IObjectToString
    {
        string BasePath { get; }
        string SecretFilePath { get; }
        string CommonReadmeSource { get; }
        string CommonBadgesSource { get; }
        string CodeOfConductSource { get; }
        string ContributingSource { get; }

        string Copyright { get; }
        SecretInfo GetSecret(string key);
        Dictionary<string, SecretInfo> GetSecrets();
        string InternalCLIDist { get; }
        string SocExePath { get; }
        IReadOnlyList<IUtityExeInstance> UtityExeInstances { get; }
        IReadOnlyList<ISolutionSettings> Solutions { get; }
        IReadOnlyList<IProjectSettings> Projects { get; }
        IReadOnlyList<IArtifactSettings> DevArtifacts { get; }
        IReadOnlyList<ILicenseSettings> Licenses { get; }

        ISolutionSettings GetSolution(KindOfProject kind);
        IReadOnlyList<ISolutionSettings> GetSolutions(KindOfProject kind);
        IReadOnlyList<ISolutionSettings> GetSolutionsWithMaintainedReleases();
        IReadOnlyList<ISolutionSettings> GetSolutionsWithMaintainedVersionsInCSharpProjects();
        IReadOnlyList<ISolutionSettings> GetSolutionsWhichUseCommonPakage();
        IReadOnlyList<ISolutionSettings> GetUnityExampleSolutions();
        IReadOnlyList<ISolutionSettings> GetCSharpSolutions();
        IReadOnlyList<ISolutionSettings> GetCSharpSolutionsWhichUseNuGetPakages();

        IProjectSettings GetProject(KindOfProject kind);
        IReadOnlyList<IProjectSettings> GetProjects(KindOfProject kind);

        IArtifactSettings GetDevArtifact(KindOfArtifact kind);
        IReadOnlyList<IArtifactSettings> GetDevArtifacts(KindOfArtifact kind);

        ILicenseSettings GetLicense(string name);
    }
}
