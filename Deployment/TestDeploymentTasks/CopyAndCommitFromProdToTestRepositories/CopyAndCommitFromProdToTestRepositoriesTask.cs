using Deployment.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.TestDeploymentTasks.CopyAndCommitFromProdToTestRepositories
{
    public class CopyAndCommitFromProdToTestRepositoriesTask : BaseDeploymentTask
    {
        public CopyAndCommitFromProdToTestRepositoriesTask()
            : this(0u)
        {
        }

        public CopyAndCommitFromProdToTestRepositoriesTask(uint deep)
            : base(null, deep)            
        {
        }
    }
}
