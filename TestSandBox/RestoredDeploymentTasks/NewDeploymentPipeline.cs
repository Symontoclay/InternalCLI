using CommonUtils.DebugHelpers;
using Deployment;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class NewDeploymentPipeline: INewDeploymentPipeline
    {
        public NewDeploymentPipeline(INewDeploymentPipelineContext context)
        {
            Context = context;
        }

        public NewDeploymentPipeline(NewDeploymentPipelineOptions options = null)
        {
#if DEBUG
            _logger.Info($"options = {options}");
#endif

            options ??= new NewDeploymentPipelineOptions();

#if DEBUG
            _logger.Info($"options (after) = {options}");
#endif

            Context = new NewDeploymentPipelineContext(options);

#if DEBUG
            _logger.Info($"Context = {Context}");
#endif
        }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        public INewDeploymentPipelineContext Context { get; }

        /// <inheritdoc/>
        public void Add(INewDeploymentTask deploymentTask)
        {
            deploymentTask.ValidateOptions();

            _deploymentTasksList.Add(deploymentTask);
        }

        private List<INewDeploymentTask> _deploymentTasksList = new List<INewDeploymentTask>();

        /// <inheritdoc/>
        public bool IsValid
        {
            get
            {
                if (!_deploymentTasksList.Any())
                {
                    return true;
                }

                return _deploymentTasksList.All(p => p.IsValid.Value);
            }
        }

        /// <inheritdoc/>
        public void Run()
        {
            var stepNumber = 0;

            foreach (var deploymentTask in _deploymentTasksList)
            {
                stepNumber++;

                try
                {
                    deploymentTask.Run();
                }
                catch (Exception e)
                {
                    throw new Exception($"Error in step {stepNumber} ({deploymentTask.GetType().FullName}): {e.Message}", e);
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.PrintObjListProp(n, nameof(_deploymentTasksList), _deploymentTasksList);
            return sb.ToString();
        }
    }
}
