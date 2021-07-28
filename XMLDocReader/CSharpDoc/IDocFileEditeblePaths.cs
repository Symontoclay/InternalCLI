using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLDocReader.CSharpDoc
{
    public interface IDocFileEditeblePaths
    {
        string Href { get; set; }
        string TargetFullFileName { get; set; }
        MemberName Name { get; set; }
    }
}
