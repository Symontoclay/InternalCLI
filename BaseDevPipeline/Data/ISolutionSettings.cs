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
        string CodeOfConductSource { get; }
        string ContributingSource { get; }
    }
}
