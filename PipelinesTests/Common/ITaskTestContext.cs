using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests.Common
{
    public interface ITaskTestContext
    {
        event Action<int, Type, string> OnMessage;
        void EmitMessage(Type type, string message);
        bool EnableFailCase1 { get; set; }
    }
}
