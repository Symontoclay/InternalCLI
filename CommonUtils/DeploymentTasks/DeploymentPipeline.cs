using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.DeploymentTasks
{
    public class DeploymentPipeline : IDeploymentPipeline
    {
        public DeploymentPipeline(IDeploymentPipelineContext context)
        {
            Context = context;
        }

        public DeploymentPipeline(DeploymentPipelineOptions options = null)
        {
#if DEBUG
            //_logger.Info($"options = {options}");
#endif

            options ??= new DeploymentPipelineOptions();

#if DEBUG
            //_logger.Info($"options (after) = {options}");
#endif

            Context = new DeploymentPipelineContext(options);

#if DEBUG
            //_logger.Info($"Context = {Context}");
#endif
        }

        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        public IDeploymentPipelineContext Context { get; }

        /// <inheritdoc/>
        public void Add(IDeploymentTask deploymentTask)
        {
            deploymentTask.SetContext(Context);
            deploymentTask.ValidateOptions();

            _deploymentTasksList.Add(deploymentTask);
        }

        private List<IDeploymentTask> _deploymentTasksList = new List<IDeploymentTask>();

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

        public static void Run(IDeploymentTask deploymentTask)
        {
            Run(deploymentTask, null);
        }

        public static void Run(IDeploymentTask deploymentTask, DeploymentPipelineOptions options)
        {
            var deploymentPipeline = new DeploymentPipeline(options);

            deploymentPipeline.Add(deploymentTask);

            deploymentPipeline.Run();
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
