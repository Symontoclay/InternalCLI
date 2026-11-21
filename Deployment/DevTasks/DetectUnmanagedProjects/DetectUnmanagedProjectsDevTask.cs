using BaseDevPipeline;
using BaseDevPipeline.Data;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System.Collections.Generic;
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
            _logger.Info($"_options = {_options}");
#endif

            var solutionsForChecking = new List<ISolutionSettings>();
            var ignoredSolutions = new List<ISolutionSettings>();

            var solutions = ProjectsDataSourceFactory.GetSolutions();

#if DEBUG
            _logger.Info($"solutions.Count = {solutions.Count}");
#endif

            foreach (var solution in solutions) 
            {
                if(_options.ManagedSolutions.Contains(solution.Name))
                {
                    solutionsForChecking.Add(solution);
                }
                else
                {
                    ignoredSolutions.Add(solution);
                }
            }

            var ignoredSb = new StringBuilder();

            if(ignoredSolutions.Any())
            {
                ignoredSb.AppendLine("These solutions were ignored:");

                foreach (var solution in ignoredSolutions)
                {

                }
            }
            else
            {
                ignoredSb.AppendLine("All solutions were checked.");
                ignoredSb.AppendLine();
            }

            _logger.Info(ignoredSb);
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
