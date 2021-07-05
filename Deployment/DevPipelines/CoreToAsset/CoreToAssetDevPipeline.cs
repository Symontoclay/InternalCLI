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
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Begin");

            var sourceProject = ProjectsDataSource.GetProject(KindOfProject.CoreAssetLib);

            _logger.Info($"sourceProject = {sourceProject}");

            var unitySolution = ProjectsDataSource.GetSolution(KindOfProject.Unity);

            _logger.Info($"unitySolution = {unitySolution}");

            var options = new CoreToAssetTaskOptions();
            options.CoreCProjPath = sourceProject.CsProjPath;
            options.DestDir = unitySolution.SourcePath;

            _logger.Info($"options = {options}");

            Exec(new CoreToAssetTask(options));
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            throw new NotImplementedException();
        }
    }
}
