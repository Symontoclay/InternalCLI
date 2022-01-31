using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.UpdateUnityExampleRepository;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.GitTasks.Clone;
using Deployment.Tasks.GitTasks.Pull;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.DevTasks.UpdateAndCommitUnityExampleRepositories
{
    public class UpdateAndCommitUnityExampleRepositoriesDevTask : BaseDeploymentTask
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public UpdateAndCommitUnityExampleRepositoriesDevTask()
            : this(0u)
        {
        }

        public UpdateAndCommitUnityExampleRepositoriesDevTask(uint deep)
            : base(null, deep)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var settings = ProjectsDataSource.GetSymOntoClayProjectsSettings();

            var unitySolution = settings.GetSolution(KindOfProject.Unity);

            var unityExamplesSolutionsList = settings.GetUnityExampleSolutions();

            foreach (var unityExampleSolution in unityExamplesSolutionsList)
            {
#if DEBUG
                _logger.Info($"unityExampleSolution = {unityExampleSolution}");
#endif

                if(Directory.Exists(unityExampleSolution.Path))
                {
                    Exec(new PullTask(new PullTaskOptions()
                    {
                        RepositoryPath = unityExampleSolution.Path
                    }, NextDeep));
                }
                else
                {
                    Exec(new CreateDirectoryTask(new CreateDirectoryTaskOptions()
                    {
                        TargetDir = unityExampleSolution.Path,
                        SkipExistingFilesInTargetDir = false
                    }, NextDeep));

                    Exec(new CloneTask(new CloneTaskOptions()
                    {
                        RepositoryHref = unityExampleSolution.GitFileHref,
                        RepositoryPath = unityExampleSolution.Path
                    }, NextDeep));

                    unityExampleSolution.RereadUnityVersion();

#if DEBUG
                    _logger.Info($"unityExampleSolution (2) = {unityExampleSolution}");
#endif
                }

                Exec(new UpdateUnityExampleRepositoryDevTask(new UpdateUnityExampleRepositoryDevTaskOptions()
                {
                    SourceRepository = PathsHelper.Normalize(@"%USERPROFILE%\source\repos\SymOntoClayAsset"),
                    DestinationRepository = unityExampleSolution.Path
                }, NextDeep));
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Updates SymOntoClay version for all unity example repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
