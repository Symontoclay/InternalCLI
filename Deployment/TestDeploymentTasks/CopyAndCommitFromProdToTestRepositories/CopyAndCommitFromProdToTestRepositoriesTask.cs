using BaseDevPipeline;
using CommonUtils.DebugHelpers;
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

        protected override void OnRun()
        {
            var prodSettings = ProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();
            var testSettings = TestProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();

            var prodTargetSolutions = prodSettings.GetSolutionsWithMaintainedReleases();
            //var testTargetSolutionsDict = testSettings.GetSolutionsWithMaintainedReleases().ToDictionary(p => );

            foreach()
            {

            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Copy prod repositories to test repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
