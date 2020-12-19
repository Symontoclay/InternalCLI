using System;
using System.Collections.Generic;
using System.Text;

namespace XMLDocReader
{
    public interface ICodeDocument
    {
        public string Href { get; }
        public string InitialName { get; }
        public string DisplayedName { get; }
    }
}
