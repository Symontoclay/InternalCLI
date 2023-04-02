using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.CopyFromProdToTestRepositories
{
    public class CopyFromProdToTestRepositoriesTask : BaseDeploymentTask
    {
        public CopyFromProdToTestRepositoriesTask()
            : this(0u)
        {
        }

        public CopyFromProdToTestRepositoriesTask(uint deep)
            : base(null, deep)            
        {
        }
    }
}
