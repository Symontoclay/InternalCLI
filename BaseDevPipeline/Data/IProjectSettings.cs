using SymOntoClay.Common;
using System.Collections.Generic;

namespace BaseDevPipeline.Data
{
    public interface IProjectSettings : IObjectToString
    {
        ISolutionSettings Solution { get; }
        KindOfProject Kind { get; }
        string FolderName { get; }
        string Path { get; }
        string CsProjPath { get; }
        string LicenseName { get; }
        ILicenseSettings License { get; }
        IReadOnlyList<KindOfProject> ExceptKindOfSolutions { get; }
    }
}
