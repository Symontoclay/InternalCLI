﻿using CommonUtils.DebugHelpers;
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
        IReadOnlyList<ISolutionSettings> GetUnityExampleSolutions();

        IProjectSettings GetProject(KindOfProject kind);
        IReadOnlyList<IProjectSettings> GetProjects(KindOfProject kind);

        IArtifactSettings GetDevArtifact(KindOfArtifact kind);
        IReadOnlyList<IArtifactSettings> GetDevArtifacts(KindOfArtifact kind);

        ILicenseSettings GetLicense(string name);
    }
}
