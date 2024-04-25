using SymOntoClay.Common;

namespace BaseDevPipeline.Data
{
    public interface IArtifactSettings : IObjectToString
    {
        KindOfArtifact Kind { get; }
        string Path { get; }
    }
}
