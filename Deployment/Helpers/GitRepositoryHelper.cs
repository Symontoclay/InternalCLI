using CommonUtils;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class GitRepositoryHelper
    {
        public static bool IsBranchExists(string repositoryPath, string branchName)
        {
            var branchNames = GetPureLocalBranchNames(repositoryPath);
            branchNames.AddRange(GetPureRemoteBranchNames(repositoryPath));

            branchNames = branchNames.Distinct().ToList();

            return branchNames.Contains(branchName);
        }

        public static List<string> GetRawLocalBranchNames(string repositoryPath)
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(repositoryPath);

            var gitProcess = new GitProcessSyncWrapper("branch -l");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Getting local branch names in repository at path '{repositoryPath}' has been failed! The exit code is {exitCode}.");
            }

            return gitProcess.Output.ToList();
        }

        public static List<string> GetPureLocalBranchNames(string repositoryPath)
        {
            return GetRawLocalBranchNames(repositoryPath).Select(p => p.Replace("*", string.Empty).Trim()).Distinct().ToList();
        }

        public static List<string> GetRawRemoteBranchNames(string repositoryPath)
        {
            var prevDir = Directory.GetCurrentDirectory();

            Directory.SetCurrentDirectory(repositoryPath);

            var gitProcess = new GitProcessSyncWrapper("branch -l -r");
            var exitCode = gitProcess.Run();

            Directory.SetCurrentDirectory(prevDir);

            if (exitCode != 0)
            {
                throw new Exception($"Getting remote branch names in repository at path '{repositoryPath}' has been failed! The exit code is {exitCode}.");
            }

            return gitProcess.Output.ToList();
        }

        public static List<string> GetPureRemoteBranchNames(string repositoryPath)
        {
            return GetRawRemoteBranchNames(repositoryPath).Where(p => !p.Contains("->")).Select(p => p.Replace("origin/", string.Empty).Trim()).Distinct().ToList();
        }
    }
}
