﻿using CollectionsHelpers.CollectionsHelpers;
using CommonUtils.DebugHelpers;
using Deployment;
using Deployment.Tasks;
using dotless.Core.Parser.Infrastructure;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSandBox.RestoredDeploymentTasks.Serialization;

namespace TestSandBox.RestoredDeploymentTasks
{
    public abstract class NewBaseDeploymentTask : INewDeploymentTask
    {
        protected NewBaseDeploymentTask(INewDeploymentPipelineContext context, string key, bool shouldBeSkeepedDuringRestoring, IObjectToString options, INewDeploymentTask parentTask)
        {
#if DEBUG
            //_logger.Info($"key = '{key}'");
            //_logger.Info($"shouldBeSkeepedDuringRestoring = {shouldBeSkeepedDuringRestoring}");
#endif

            _context = context;
            _key = key;
            _shouldBeSkeepedDuringRestoring = shouldBeSkeepedDuringRestoring;
            _options = options;
            _parentTask = parentTask;
            _deep = parentTask?.NextDeep ?? 0u;
        }

        protected readonly INewDeploymentPipelineContext _context;
        private readonly string _key;
        private readonly bool _shouldBeSkeepedDuringRestoring;
        private readonly IObjectToString _options;
        private INewDeploymentTask _parentTask;
        private readonly uint _deep;
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private bool? _isValid;
        private List<string> _validationMessages = new List<string>();
        private NewDeploymentTaskRunInfo _currentDeploymentTaskRunInfo;

        /// <inheritdoc/>
        public string Key => _key;

        /// <inheritdoc/>
        public void SetParentTask(INewDeploymentTask parentTask)
        {
            _parentTask = parentTask;
        }

        /// <inheritdoc/>
        public NewDeploymentTaskRunInfo GetChildDeploymentTaskRunInfo(string key)
        {
            return _currentDeploymentTaskRunInfo?.SubTaks.SingleOrDefault(p => p.Key == key);
        }

        /// <inheritdoc/>
        public bool ContainsChild(string key)
        {
            return _currentDeploymentTaskRunInfo?.SubTaks.Any(p => p.Key == key) ?? false;
        }

        /// <inheritdoc/>
        public void AddChildDeploymentTaskRunInfo(NewDeploymentTaskRunInfo item)
        {
            _currentDeploymentTaskRunInfo?.SubTaks.Add(item);
        }

        /// <inheritdoc/>
        public uint Deep => _deep;

        /// <inheritdoc/>
        public uint NextDeep => _deep + 1;

        /// <inheritdoc/>
        public bool? IsValid => _isValid;

        public IReadOnlyList<string> ValidationMessages => _validationMessages;

        /// <inheritdoc/>
        public void ValidateOptions()
        {
            OnValidateOptions();

            if (!_isValid.HasValue)
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
                _currentDeploymentTaskRunInfo = _context.GetDeploymentTaskRunInfo(_key, _parentTask);

#if DEBUG
                //_logger.Info($"_currentDeploymentTaskRunInfo = {_currentDeploymentTaskRunInfo}");
#endif

                if (_currentDeploymentTaskRunInfo.IsFinished ?? false)
                {
#if DEBUG
                    //_logger.Info($"_shouldBeSkeepedDuringRestoring = {_shouldBeSkeepedDuringRestoring}");
#endif

                    if (_shouldBeSkeepedDuringRestoring)
                    {
                        _logger.Info($"{spaces}{GetType().Name} (Key: '{_key}') is skeeped.");
                        if (_options != null)
                        {
                            _logger.Info($"{_options.ToString(n + 4)}");
                        }

                        return;
                    }
                }

                _logger.Info($"{spaces}{GetType().Name} (Key: '{_key}') started.");
                if (_options != null)
                {
                    _logger.Info($"{_options.ToString(n + 4)}");
                }

                CheckValidationOptions();

                OnRun();

                _currentDeploymentTaskRunInfo.IsFinished = true;
                _context.SaveDeploymentTaskRunInfo();

                _logger.Info($"{spaces}{GetType().Name} (Key: '{_key}') finished");
            }
            catch (Exception e)
            {
                _logger.Info($"{spaces}Error in {GetType().Name} (Key: '{_key}') with options:");
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
            if (options == null)
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
            if (list.IsNullOrEmpty())
            {
                AddValidationMessage($"The {optionName} can not be null or empty!");
            }
        }

        protected virtual void CheckValidationOptions()
        {
            if (_validationMessages.Any())
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

        protected void Exec(INewDeploymentTask deploymentTask)
        {
            var deploymentPipeline = new NewDeploymentPipeline(_context);

            deploymentPipeline.Add(deploymentTask);

            deploymentPipeline.Run();
        }
    }
}
