using CommonUtils;
using CommonUtils.DeploymentTasks;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class SubItemNewDeploymentTask : BaseDeploymentTask
    {
        public SubItemNewDeploymentTask(SubItemNewDeploymentTaskOptions options)
            : this(options, null)
        {
        }

        public SubItemNewDeploymentTask(SubItemNewDeploymentTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.DirectoryName), false, options, parentTask)
        {
            _options = options;
        }

        private readonly SubItemNewDeploymentTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnRun()
        {
            _logger.Info("Being");
            _logger.Info($"_options.DirectoryName = {_options.DirectoryName}");
            _n++;
            _logger.Info($"_n = {_n}");
            if (_n == 5)
            {
                //throw new NotImplementedException();
            }
            _logger.Info("End");
        }

        private static int _n;

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Do something with '{_options.DirectoryName}'.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
