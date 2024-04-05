using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelinesTests.Common
{
    public class TaskTestContext: ITaskTestContext
    {
#if DEBUG
        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public event Action<int, Type, string> OnMessage;

        private int n;

        void ITaskTestContext.EmitMessage(Type type, string message)
        {
            n++;

#if DEBUG
            //_logger.Info(type.Name);
            //_logger.Info(message);
#endif

            OnMessage?.Invoke(n, type, message);
        }

        public bool EnableFailCase1 { get; set; }
    }
}
