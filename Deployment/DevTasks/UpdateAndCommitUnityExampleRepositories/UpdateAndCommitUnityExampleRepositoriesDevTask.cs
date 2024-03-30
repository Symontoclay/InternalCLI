using BaseDevPipeline;
using CommonUtils.DebugHelpers;
using Deployment.DevTasks.UpdateUnityExampleRepository;
using Deployment.Tasks;
using Deployment.Tasks.DirectoriesTasks.CreateDirectory;
using Deployment.Tasks.GitTasks.Clone;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
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
    public class UpdateAndCommitUnityExampleRepositoriesDevTask : OldBaseDeploymentTask
    {
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
            var settings = ProjectsDataSourceFactory.GetSymOntoClayProjectsSettings();

            var unitySolution = settings.GetSolution(KindOfProject.Unity);

            var unityExamplesSolutionsList = settings.GetUnityExampleSolutions();

            foreach (var unityExampleSolution in unityExamplesSolutionsList)
            {
                if (Directory.Exists(unityExampleSolution.Path))
                {
                    Exec(new PullTask(new PullTaskOptions()
                    {
                        RepositoryPath = unityExampleSolution.Path
                    }, NextDeep));
                }
                else
                {
                    var baseReposPath = Path.GetDirectoryName(unityExampleSolution.Path);

                    Exec(new CloneTask(new CloneTaskOptions()
                    {
                        RepositoryHref = unityExampleSolution.GitFileHref,
                        RepositoryPath = baseReposPath
                    }, NextDeep));

                    unityExampleSolution.RereadUnityVersion();
                }

                Exec(new UpdateUnityExampleRepositoryDevTask(new UpdateUnityExampleRepositoryDevTaskOptions()
                {
                    SourceRepository = unitySolution.Path,
                    DestinationRepository = unityExampleSolution.Path
                }, NextDeep));

                Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                {
                    Message = "SymOntoClay version has been updated",
                    RepositoryPaths = new List<string> { unityExampleSolution.Path }
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
