using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks.Serialization;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonUtils.DeploymentTasks
{
    public class DeploymentPipelineContext : IDeploymentPipelineContext
    {
        public DeploymentPipelineContext(DeploymentPipelineOptions options)
        {
#if DEBUG
            //_logger.Info($"options = {options}");
#endif

            _useAutorestoring = options.UseAutorestoring;

            if (_useAutorestoring)
            {
                if (string.IsNullOrWhiteSpace(options.Prefix))
                {
                    _pipelineInfoFileName = _pipelineInfoInitialFileName;
                    _currentRunInfoFileName = _currentRunInfoInitialFileName;
                }
                else
                {
                    _pipelineInfoFileName = $"{options.Prefix}_{_pipelineInfoInitialFileName}";
                    _currentRunInfoFileName = $"{options.Prefix}_{_currentRunInfoInitialFileName}";
                }

                _directoryForAutorestoring = EVPath.Normalize(options.DirectoryForAutorestoring);

                if (string.IsNullOrWhiteSpace(_directoryForAutorestoring))
                {
                    _directoryForAutorestoring = Directory.GetCurrentDirectory();
                }
                else
                {
                    Directory.CreateDirectory(_directoryForAutorestoring);
                }

                _pipelineInfoFileFullName = Path.Combine(_directoryForAutorestoring, _pipelineInfoFileName);

#if DEBUG
                //_logger.Info($"_pipelineInfoFileFullName = {_pipelineInfoFileFullName}");
#endif

                if (File.Exists(_pipelineInfoFileFullName))
                {
                    _pipelineInfo = JsonConvert.DeserializeObject<PipelineInfo>(File.ReadAllText(_pipelineInfoFileFullName));

#if DEBUG
                    //_logger.Info($"_pipelineInfo = {_pipelineInfo}");
#endif

                    if (_pipelineInfo.IsFinished ?? false)
                    {
                        InitNewSession();
                    }
                    else
                    {
                        if (options.StartFromBeginning)
                        {
                            InitNewSession();
                        }
                        else
                        {
                            _currentRunInfoFileFullName = _pipelineInfo.LastRunInfo;

#if DEBUG
                            //_logger.Info($"_currentRunInfoFileFullName = {_currentRunInfoFileFullName}");
#endif

                            _rootDeploymentTaskRunInfoList = JsonConvert.DeserializeObject<List<DeploymentTaskRunInfo>>(File.ReadAllText(_currentRunInfoFileFullName));
                        }
                    }
                }
                else
                {
                    InitNewSession();
                }
            }
        }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly bool _useAutorestoring;
        private readonly string _directoryForAutorestoring;
        private readonly string _pipelineInfoInitialFileName = "pipelineInfo.json";
        private readonly string _pipelineInfoFileName;
        private readonly string _pipelineInfoFileFullName;
        private readonly string _currentRunInfoInitialFileName = "pipelineCurentRunInfo.json";
        private readonly string _currentRunInfoFileName;
        private string _currentRunInfoFileFullName;
        private PipelineInfo _pipelineInfo;
        private List<DeploymentTaskRunInfo> _rootDeploymentTaskRunInfoList;
        private bool _isNewSession;

        /// <inheritdoc/>
        public bool IsNewSession => _isNewSession;

        private void InitNewSession()
        {
            _isNewSession = true;

            _currentRunInfoFileFullName = Path.Combine(_directoryForAutorestoring, _currentRunInfoFileName);

            _pipelineInfo = new PipelineInfo()
            {
                IsFinished = false,
                LastRunInfo = _currentRunInfoFileFullName
            };

            SavePipelineInfo();

            _rootDeploymentTaskRunInfoList = new List<DeploymentTaskRunInfo>();

            SaveDeploymentTaskRunInfo();
        }

        private void SavePipelineInfo()
        {
            File.WriteAllText(_pipelineInfoFileFullName, JsonConvert.SerializeObject(_pipelineInfo, Formatting.Indented));
        }

        /// <inheritdoc/>
        public void SaveDeploymentTaskRunInfo()
        {
            File.WriteAllText(_currentRunInfoFileFullName, JsonConvert.SerializeObject(_rootDeploymentTaskRunInfoList, Formatting.Indented));

#if DEBUG
            //_logger.Info($"_rootDeploymentTaskRunInfoList.All(p => p.IsFinished ?? false) = {_rootDeploymentTaskRunInfoList.All(p => p.IsFinished ?? false)}");
            //_logger.Info($"_rootDeploymentTaskRunInfoList = {_rootDeploymentTaskRunInfoList.WriteListToString()}");
#endif

            if (_rootDeploymentTaskRunInfoList.Any() && _rootDeploymentTaskRunInfoList.All(p => p.IsFinished ?? false))
            {
                _pipelineInfo.IsFinished = true;

                SavePipelineInfo();
            }

#if DEBUG
            //_logger.Info($"_pipelineInfo = {_pipelineInfo}");
#endif
        }

        /// <inheritdoc/>
        public DeploymentTaskRunInfo GetDeploymentTaskRunInfo(string key, IDeploymentTask parentTask)
        {
#if DEBUG
            //_logger.Info($"key = {key}");
            //_logger.Info($"parentTask = {parentTask}");
#endif

            if (!_useAutorestoring)
            {
                return new DeploymentTaskRunInfo()
                {
                    Key = key,
                    IsFinished = false,
                    SubTaks = new List<DeploymentTaskRunInfo>()
                };
            }

            if (parentTask == null)
            {
                if (_isNewSession)
                {
                    if (_rootDeploymentTaskRunInfoList.Any(p => p.Key == key))
                    {
                        throw new Exception($"The key '{key}' already exists in root list.");
                    }

                    var item = new DeploymentTaskRunInfo()
                    {
                        Key = key,
                        IsFinished = false,
                        SubTaks = new List<DeploymentTaskRunInfo>()
                    };

                    _rootDeploymentTaskRunInfoList.Add(item);

#if DEBUG
                    //_logger.Info($"_rootDeploymentTaskRunInfoList = {_rootDeploymentTaskRunInfoList.WriteListToString()}");
#endif

                    return item;
                }
                else
                {
                    var item = _rootDeploymentTaskRunInfoList.SingleOrDefault(p => p.Key == key);

                    if (item == null)
                    {
                        item = new DeploymentTaskRunInfo()
                        {
                            Key = key,
                            IsFinished = false,
                            SubTaks = new List<DeploymentTaskRunInfo>()
                        };

                        _rootDeploymentTaskRunInfoList.Add(item);
                    }

#if DEBUG
                    //_logger.Info($"_rootDeploymentTaskRunInfoList = {_rootDeploymentTaskRunInfoList.WriteListToString()}");
#endif

                    return item;
                }
            }
            else
            {
                if (_isNewSession)
                {
                    if (parentTask.ContainsChild(key))
                    {
                        throw new Exception($"The key '{key}' already exists in parent task with key '{parentTask.Key}'.");
                    }

                    var item = new DeploymentTaskRunInfo()
                    {
                        Key = key,
                        IsFinished = false,
                        SubTaks = new List<DeploymentTaskRunInfo>()
                    };

                    parentTask.AddChildDeploymentTaskRunInfo(item);

#if DEBUG
                    //_logger.Info($"_rootDeploymentTaskRunInfoList = {_rootDeploymentTaskRunInfoList.WriteListToString()}");
#endif

                    return item;
                }
                else
                {
                    var item = parentTask.GetChildDeploymentTaskRunInfo(key);

                    if (item == null)
                    {
                        item = new DeploymentTaskRunInfo()
                        {
                            Key = key,
                            IsFinished = false,
                            SubTaks = new List<DeploymentTaskRunInfo>()
                        };

                        parentTask.AddChildDeploymentTaskRunInfo(item);
                    }

#if DEBUG
                    //_logger.Info($"_rootDeploymentTaskRunInfoList = {_rootDeploymentTaskRunInfoList.WriteListToString()}");
#endif

                    return item;
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
            sb.AppendLine($"{spaces}{nameof(_useAutorestoring)} = {_useAutorestoring}");
            sb.AppendLine($"{spaces}{nameof(_directoryForAutorestoring)} = {_directoryForAutorestoring}");
            sb.AppendLine($"{spaces}{nameof(_pipelineInfoFileName)} = {_pipelineInfoFileName}");
            sb.AppendLine($"{spaces}{nameof(_pipelineInfoFileFullName)} = {_pipelineInfoFileFullName}");
            sb.PrintObjProp(n, nameof(_pipelineInfo), _pipelineInfo);
            return sb.ToString();
        }
    }
}
