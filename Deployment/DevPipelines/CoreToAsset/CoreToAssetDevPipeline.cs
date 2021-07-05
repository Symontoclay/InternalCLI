using BaseDevPipeline;
using Deployment.DevTasks.CoreToAsset;
using Deployment.Tasks;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevPipelines.CoreToAsset
{
    public class CoreToAssetDevPipeline : BaseDeploymentTask
    {
        /// <inheritdoc/>
        protected override void OnRun()
        {
            var sourceProject = ProjectsDataSource.GetProject(KindOfProject.CoreAssetLib);

            var unitySolution = ProjectsDataSource.GetSolution(KindOfProject.Unity);

            var options = new CoreToAssetTaskOptions();
            options.CoreCProjPath = sourceProject.CsProjPath;
            options.DestDir = unitySolution.SourcePath;

            Exec(new CoreToAssetTask(options));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            throw new NotImplementedException();
        }
    }
}
