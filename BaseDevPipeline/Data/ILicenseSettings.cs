using SymOntoClay.Common;

namespace BaseDevPipeline.Data
{
    public interface ILicenseSettings : IObjectToString
    {
        string Name { get; }
        string HeaderFileName { get; }
        string HeaderContent { get; }
        string FileName { get; }
        string Content { get; }
    }
}
