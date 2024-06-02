using SymOntoClay.Common;

namespace BaseDevPipeline.Data
{
    public interface ITempSettings : IObjectToString
    {
        string Dir { get; }
        bool ClearOnDispose { get; }
    }
}
