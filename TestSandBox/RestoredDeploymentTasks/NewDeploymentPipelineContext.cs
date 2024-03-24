using CommonUtils;
using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSandBox.RestoredDeploymentTasks.Serialization;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class NewDeploymentPipelineContext: INewDeploymentPipelineContext
    {
        public NewDeploymentPipelineContext(NewDeploymentPipelineOptions options)
        {
#if DEBUG
            _logger.Info($"options = {options}");
#endif

            _useAutorestoring = options.UseAutorestoring;

            if(_useAutorestoring)
            {
                _directoryForAutorestoring = EVPath.Normalize(options.DirectoryForAutorestoring);

                if(string.IsNullOrWhiteSpace(_directoryForAutorestoring))
                {
                    _directoryForAutorestoring = Directory.GetCurrentDirectory();
                }
                else
                {
                    Directory.CreateDirectory(_directoryForAutorestoring);
                }

                
            }
        }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly bool _useAutorestoring;
        private readonly string _directoryForAutorestoring;
        private readonly string _pipelineInfoFileName = "pipelineInfoName.json";
        private readonly string _pipelineInfoFileFullName;
        private readonly NewPipelineInfo _pipelineInfo;

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
            sb.AppendLine($"{spaces}{nameof(_useAutorestoring)} = {_useAutorestoring}");
            sb.AppendLine($"{spaces}{nameof(_directoryForAutorestoring)} = {_directoryForAutorestoring}");
            sb.AppendLine($"{spaces}{nameof(_pipelineInfoFileName)} = {_pipelineInfoFileName}");
            sb.AppendLine($"{spaces}{nameof(_pipelineInfoFileFullName)} = {_pipelineInfoFileFullName}");
            sb.PrintObjProp(n, nameof(_pipelineInfo), _pipelineInfo);
            return sb.ToString();
        }
    }
}
