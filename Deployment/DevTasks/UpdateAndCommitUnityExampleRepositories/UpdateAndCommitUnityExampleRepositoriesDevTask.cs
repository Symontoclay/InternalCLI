using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.DevTasks.UpdateUnityExampleRepository;
using Deployment.Tasks.GitTasks.Clone;
using Deployment.Tasks.GitTasks.CommitAllAndPush;
using Deployment.Tasks.GitTasks.Pull;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Deployment.DevTasks.UpdateAndCommitUnityExampleRepositories
{
    public class UpdateAndCommitUnityExampleRepositoriesDevTask : BaseDeploymentTask
    {
        public UpdateAndCommitUnityExampleRepositoriesDevTask()
            : this(null)
        {
        }

        public UpdateAndCommitUnityExampleRepositoriesDevTask(IDeploymentTask parentTask)
            : base("68D4C439-4465-431A-9B52-6C0B8CF9BD41", false, null, parentTask)
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
                    }, this));
                }
                else
                {
                    var baseReposPath = Path.GetDirectoryName(unityExampleSolution.Path);

                    Exec(new CloneTask(new CloneTaskOptions()
                    {
                        RepositoryHref = unityExampleSolution.GitFileHref,
                        RepositoryPath = baseReposPath
                    }, this));

                    unityExampleSolution.RereadUnityVersion();
                }

                Exec(new UpdateUnityExampleRepositoryDevTask(new UpdateUnityExampleRepositoryDevTaskOptions()
                {
                    SourceRepository = unitySolution.Path,
                    DestinationRepository = unityExampleSolution.Path
                }, this));

                Exec(new CommitAllAndPushTask(new CommitAllAndPushTaskOptions()
                {
                    Message = "SymOntoClay version has been updated",
                    RepositoryPaths = new List<string> { unityExampleSolution.Path }
                }, this));
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
