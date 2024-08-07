﻿using BaseDevPipeline;
using CommonUtils.DeploymentTasks;
using Deployment.Helpers;
using Octokit;
using SymOntoClay.Common.DebugHelpers;
using System.Linq;
using System.Text;

namespace Deployment.TestDeploymentTasks.RemoveReleasesFromTestRepositories
{
    public class RemoveReleasesFromTestRepositoriesTask : BaseDeploymentTask
    {
        public RemoveReleasesFromTestRepositoriesTask()
            : this(null)
        {
        }

        public RemoveReleasesFromTestRepositoriesTask(IDeploymentTask parentTask)
            : base("A851CFDF-7356-4A4C-A5EF-6B9115A27A1C", true, null, parentTask)
        {
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var testSettings = TestProjectsDataSource.Instance.GetSymOntoClayProjectsSettings();
            var testTargetSolutions = testSettings.GetSolutionsWithMaintainedReleases();

            var client = new GitHubClient(new ProductHeaderValue("SymOntoClay-InternalCLI"));

            var token = testSettings.GetSecret(GitHubTokenHelper.GitHubTokenKey);

            var tokenAuth = new Credentials(token.Value);
            client.Credentials = tokenAuth;

#if DEBUG
            //_logger.Info($"testTargetSolutions.Count = {testTargetSolutions.Count}");
#endif

            foreach (var testTargetSolution in testTargetSolutions)
            {
#if DEBUG
                //_logger.Info($"testTargetSolution.OwnerName = {testTargetSolution.OwnerName}");
                //_logger.Info($"testTargetSolution.RepositoryName = {testTargetSolution.RepositoryName}");
#endif

                var releasesListTask = client.Repository.Release.GetAll(testTargetSolution.OwnerName, testTargetSolution.RepositoryName);

                var releasesList = releasesListTask.Result;

#if DEBUG
                //_logger.Info($"releasesList.Count = {releasesList.Count}");
#endif

//                foreach (var release in releasesList)
//                {
//#if DEBUG
//                    _logger.Info($"release.Id = {release.Id}");
//                    _logger.Info($"release.Name = {release.Name}");
//                    _logger.Info($"release.TagName = {release.TagName}");
//#endif
//                }

                var releasesIdList = releasesList.Select(p => p.Id).ToList();

                foreach(var releaseId in releasesIdList)
                {
#if DEBUG
                    //_logger.Info($"releaseId = {releaseId}");
#endif

                    var task = client.Repository.Release.Delete(testTargetSolution.OwnerName, testTargetSolution.RepositoryName, releaseId);
                    task.Wait();
                }
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Remove releases from test repositories.");
            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
