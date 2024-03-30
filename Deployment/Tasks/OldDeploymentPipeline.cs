using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks
{
    [Obsolete("It should be removed", true)]
    public class OldDeploymentPipeline: IOldDeploymentPipeline
    {
        /// <inheritdoc/>
        public void Add(IOldDeploymentTask deploymentTask)
        {
            deploymentTask.ValidateOptions();

            _deploymentTasksList.Add(deploymentTask);
        }

        private List<IOldDeploymentTask> _deploymentTasksList = new List<IOldDeploymentTask>();

        public bool IsValid
        {
            get
            {
                if(!_deploymentTasksList.Any())
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
                catch(Exception e)
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
