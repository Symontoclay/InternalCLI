using BaseDevPipeline;
using BaseDevPipeline.Data;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.DevTasks.DetectUnmanagedProjects
{
    public class DetectUnmanagedProjectsDevTask : BaseDeploymentTask
    {
        public DetectUnmanagedProjectsDevTask(DetectUnmanagedProjectsDevTaskOptions options)
            : this(options, null)
        {
        }

        public DetectUnmanagedProjectsDevTask(DetectUnmanagedProjectsDevTaskOptions options, IDeploymentTask parentTask)
            : base("9E9285BA-92A2-48DB-AE54-251CAAD42F83", false, options, parentTask)
        {
            _options = options;
        }

        private readonly DetectUnmanagedProjectsDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
#if DEBUG
            //_logger.Info($"_options = {_options}");
#endif

            var solutionsForChecking = new List<ISolutionSettings>();
            var ignoredSolutions = new List<ISolutionSettings>();

            var solutions = ProjectsDataSourceFactory.GetSolutions();

#if DEBUG
            //_logger.Info($"solutions.Count = {solutions.Count}");
#endif

            foreach (var solution in solutions) 
            {
                if(_options.CheckedSolutions.Contains(solution.Name))
                {
                    solutionsForChecking.Add(solution);
                }
                else
                {
                    ignoredSolutions.Add(solution);
                }
            }

            var indentation_1 = DisplayHelper.IndentationStep;
            var spaces_1 = DisplayHelper.Spaces(indentation_1);

            var sb = new StringBuilder();

            if(ignoredSolutions.Any())
            {
                sb.AppendLine("These solutions were ignored:");

                foreach (var solution in ignoredSolutions)
                {
                    sb.AppendLine($"{spaces_1}{solution.Name}");
                }

                sb.AppendLine();
            }
            else
            {
                sb.AppendLine("All solutions were checked.");
                sb.AppendLine();
            }

            if(solutionsForChecking.Any())
            {
                sb.AppendLine("These solutions were checked:");

                foreach (var solution in solutionsForChecking)
                {
                    sb.AppendLine($"{spaces_1}{solution.Name}");

                    CheckSolution(solution, sb, indentation_1);

                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("Nothing was checked.");
                sb.AppendLine();
            }

            _logger.Info(sb);
        }

        private void CheckSolution(ISolutionSettings solution, StringBuilder sb, uint parentIndentation)
        {
            var solutionPath = solution.Path;

#if DEBUG
            //_logger.Info($"solutionPath = {solutionPath}");
#endif

            var parentSpaces = DisplayHelper.Spaces(parentIndentation);
            var indentation = parentIndentation + DisplayHelper.IndentationStep;
            var spaces = DisplayHelper.Spaces(indentation);

            var projectFilesList = new List<FileInfo>();

            FindProjectsFiles(new DirectoryInfo(solutionPath), projectFilesList);

            if(!projectFilesList.Any())
            {
                sb.AppendLine($"{parentSpaces}Not any c# projects.");
                return;
            }

            var managedCsProjPaths = solution.Projects.Select(p => p.CsProjPath.ToLower());

            var notManagedPprojectFiles = projectFilesList.Where(p => !managedCsProjPaths.Contains(p.FullName.Replace("\\", "/").ToLower())).ToList();

            if(notManagedPprojectFiles.Any())
            {
                sb.AppendLine($"{parentSpaces}These c# projects are not managed:");

                foreach (var projectFile in notManagedPprojectFiles)
                {
                    sb.AppendLine($"{spaces}{projectFile.FullName}");
                }
            }
            else
            {
                sb.AppendLine($"{parentSpaces}All c# projects are managed.");
            }
        }

        private void FindProjectsFiles(DirectoryInfo parentDirectory, List<FileInfo> result)
        {
            var projectFiles = parentDirectory.GetFiles("*.csproj");

            if(projectFiles.Any())
            {
                result.AddRange(projectFiles);

                return;
            }

            var directories = parentDirectory.GetDirectories();

            foreach (var directory in directories) 
            {
#if DEBUG
                //_logger.Info($"directory.FullName = {directory.FullName}");
#endif

                FindProjectsFiles(directory, result);
            }
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Checks the solutions listed in the `ProjectsDataSource.json` file to ensure that the projects they contain are present in `ProjectsDataSource.json`.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
