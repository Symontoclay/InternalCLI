using SymOntoClay.Common;

namespace BaseDevPipeline.Data
{
    public interface IUtityExeInstance : IObjectToString
    {
        string Version { get; }
        string Path { get; }
    }
}
