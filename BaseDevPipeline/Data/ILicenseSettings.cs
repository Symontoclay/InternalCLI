using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
