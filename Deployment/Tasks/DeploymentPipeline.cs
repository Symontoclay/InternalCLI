﻿using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks
{
    public class DeploymentPipeline: IDeploymentPipeline
    {
        /// <inheritdoc/>
        public void Add(IDeploymentTask deploymentTask)
        {
            deploymentTask.ValidateOptions();

            _deploymentTasksList.Add(deploymentTask);
        }

        private List<IDeploymentTask> _deploymentTasksList = new List<IDeploymentTask>();

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
            foreach (var deploymentTask in _deploymentTasksList)
            {
                deploymentTask.Run();
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
            return sb.ToString();
        }
    }
}
