using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Deployment.Tasks
{
    public abstract class BaseDeploymentTask : IDeploymentTask
    {
        protected BaseDeploymentTask(IObjectToString options, uint deep)
        {
            _options = options;
            _deep = deep;
        }

        private readonly IObjectToString _options;
        private readonly uint _deep;
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private bool? _isValid;
        private List<string> _validationMessages = new List<string>();

        public uint Deep => _deep;
        public uint NextDeep => _deep + 1;

        /// <inheritdoc/>
        public bool? IsValid => _isValid;

        public IReadOnlyList<string> ValidationMessages => _validationMessages;

        /// <inheritdoc/>
        public void ValidateOptions()
        {
            OnValidateOptions();

            if(!_isValid.HasValue)
            {
                _isValid = true;
            }
        }

        /// <inheritdoc/>
        public virtual void Run()
        {
            var n = Deep * 4;

            var spaces = DisplayHelper.Spaces(n);

            try
            {
                _logger.Info($"{spaces}{GetType().Name} started.");
                if(_options != null)
                {
                    _logger.Info($"{_options.ToString(n + 4)}");
                }                

                CheckValidationOptions();

                OnRun();

                _logger.Info($"{spaces}{GetType().Name} finished");
            }
            catch(Exception e)
            {
                _logger.Info($"{spaces}Error in {GetType().Name} with options:");
                _logger.Info(_options?.ToString(n + 4));
                _logger.Info(e);

                throw;
            }
        }

        protected virtual void OnValidateOptions()
        {
        }

        protected void AddValidationMessage(string validationMessage)
        {
            _isValid = false;
            _validationMessages.Add(validationMessage);
        }

        protected void ValidateOptionsAsNonNull(object options)
        {
            if(options == null)
            {
                AddValidationMessage("The options can not be null!");
            }
        }

        protected void ValidateValueAsNonNull(string optionName, object optionValue)
        {
            if (optionValue == null)
            {
                AddValidationMessage($"The {optionName} can not be null!");
            }
        }

        protected void ValidateDirectory(string optionName, string optionValue)
        {
            if (string.IsNullOrWhiteSpace(optionValue))
            {
                AddValidationMessage($"The {optionName} can not be null or empty!");
            }
        }

        protected void ValidateFileName(string optionName, string optionValue)
        {
            if (string.IsNullOrWhiteSpace(optionValue))
            {
                AddValidationMessage($"The {optionName} can not be null or empty!");
            }
        }

        protected void ValidateStringValueAsNonNullOrEmpty(string optionName, string optionValue)
        {
            if (string.IsNullOrWhiteSpace(optionValue))
            {
                AddValidationMessage($"The {optionName} can not be null or empty!");
            }
        }

        protected void ValidateList<T>(string optionName, IEnumerable<T> list)
        {
            if(list.IsNullOrEmpty())
            {
                AddValidationMessage($"The {optionName} can not be null or empty!");
            }
        }

        protected virtual void CheckValidationOptions()
        {
            if(_validationMessages.Any())
            {
                throw new Exception(_validationMessages.First());
            }
        }

        protected abstract void OnRun();

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
            return PropertiesToString(n);
        }

        protected abstract string PropertiesToString(uint n);

        protected string PrintValidation(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var next_N = n + 4;
            var nextSpaces = DisplayHelper.Spaces(next_N);

            var sb = new StringBuilder();

            if (_isValid.HasValue)
            {
                if (!_isValid.Value)
                {
                    sb.AppendLine();
                    sb.AppendLine($"{spaces}The task is invalid:");
                    foreach (var validationMessage in _validationMessages)
                    {
                        sb.AppendLine($"{nextSpaces}{validationMessage}");
                    }
                }
            }
            else
            {
                sb.AppendLine($"{spaces}The task is not validated.");
            }

            return sb.ToString();
        }

        protected void Exec(IDeploymentTask deploymentTask)
        {
            var deploymentPipeline = new DeploymentPipeline();

            deploymentPipeline.Add(deploymentTask);

            deploymentPipeline.Run();
        }
    }
}
