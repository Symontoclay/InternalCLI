using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.Readme
{
    public class ReadmeTask
    {
        public ReadmeTask(ReadmeTaskOptions options)
        {
            _options = options;
        }

        private readonly ReadmeTaskOptions _options;
    }
}
