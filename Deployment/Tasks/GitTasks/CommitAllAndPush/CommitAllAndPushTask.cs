﻿using CommonUtils.DebugHelpers;
using Deployment.Helpers;
using Deployment.Tasks.GitTasks.Add;
using Deployment.Tasks.GitTasks.Commit;
using Deployment.Tasks.GitTasks.Pull;
using Deployment.Tasks.GitTasks.Push;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.GitTasks.CommitAllAndPush
{
    public class CommitAllAndPushTask : BaseDeploymentTask
    {
        public CommitAllAndPushTask(CommitAllAndPushTaskOptions options)
            : this(options, 0u)
        {
        }

        public CommitAllAndPushTask(CommitAllAndPushTaskOptions options, uint deep)
            : base(options, deep)
        {
            _options = options;
        }

        private readonly CommitAllAndPushTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateList(nameof(_options.RepositoryPaths), _options.RepositoryPaths);
            ValidateStringValueAsNonNullOrEmpty(nameof(_options.Message), _options.Message);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var reposForCommit = new List<string>();

            foreach(var repositoryPath in _options.RepositoryPaths)
            {
                var filesList = GitRepositoryHelper.GetRepositoryFileInfoList(repositoryPath);

                if(!filesList.Any())
                {
                    continue;
                }

#if DEBUG
                //_logger.Info($"filesList = {JsonConvert.SerializeObject(filesList, Formatting.Indented)}");
#endif

                reposForCommit.Add(repositoryPath);

                var untrackedFilesList = filesList.Where(p => p.Status == GitRepositoryFileStatus.Untracked);

                foreach(var untrackedFile in untrackedFilesList)
                {
                    Exec(new AddTask(new AddTaskOptions() { 
                        RepositoryPath = repositoryPath,
                        RelativeFileName = untrackedFile.RelativePath
                    }, NextDeep));
                }
            }

#if DEBUG
            //_logger.Info($"reposForCommit = {JsonConvert.SerializeObject(reposForCommit, Formatting.Indented)}");
#endif

            foreach (var repositoryPath in reposForCommit)
            {
                Exec(new CommitTask(new CommitTaskOptions()
                {
                    RepositoryPath = repositoryPath,
                    Message = _options.Message
                }, NextDeep));
            }

            foreach (var repositoryPath in _options.RepositoryPaths)
            {
                Exec(new PullTask(new PullTaskOptions()
                {
                    RepositoryPath = repositoryPath
                }, NextDeep));
            }

            foreach (var repositoryPath in _options.RepositoryPaths)
            {
                Exec(new PushTask(new PushTaskOptions()
                {
                    RepositoryPath = repositoryPath
                }, NextDeep));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);

            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Commits changes to local branch in repositories at paths '{string.Join(',', _options.RepositoryPaths)}'.");
            sb.AppendLine($"{spaces}The reason of the commit: '{_options.Message}'.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
