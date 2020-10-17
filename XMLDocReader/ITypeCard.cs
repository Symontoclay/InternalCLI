using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader
{
    public interface ITypeCard
    {
        KindOfType KindOfType { get; }
        Type Type { get; }
        bool IsPublic { get; }
    }
}
