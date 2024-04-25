using SymOntoClay.Common;
using System.Collections.Generic;

namespace BaseDevPipeline.Data
{
    public interface ISolutionSettings : IObjectToString
    {
        string Name { get; }
        KindOfProject Kind { get; }
        string Href { get; }
        string GitFileHref { get; }
        string RepositoryName { get; }
        string OwnerName { get; }
        string Path { get; }
        string SlnPath { get; }
        string SourcePath { get; }
        IReadOnlyList<IProjectSettings> Projects { get; }
        string LicenseName { get; }
        ILicenseSettings License { get; }
        IReadOnlyList<KindOfArtifact> ArtifactsForDeployment { get; }
        bool EnableGenerateReadme { get; }
        string ReadmeSource { get; }
        string BadgesSource { get; }
        string FullUnityVersion { get; }
        string UnityVersion { get; }
        string UnityRelease { get; }

        void RereadUnityVersion();
    }
}
