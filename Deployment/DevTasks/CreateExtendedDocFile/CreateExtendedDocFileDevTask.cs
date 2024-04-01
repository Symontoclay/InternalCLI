using CommonUtils;
using CommonUtils.DebugHelpers;
using CommonUtils.DeploymentTasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMLDocReader;
using XMLDocReader.CSharpDoc;

namespace Deployment.DevTasks.CreateExtendedDocFile
{
    public class CreateExtendedDocFileDevTask : BaseDeploymentTask
    {
        public CreateExtendedDocFileDevTask(CreateExtendedDocFileDevTaskOptions options)
            : this(options, null)
        {
        }

        public CreateExtendedDocFileDevTask(CreateExtendedDocFileDevTaskOptions options, IDeploymentTask parentTask)
            : base(MD5Helper.GetHash(options.XmlDocFile), false, options, parentTask)
        {
            _options = options;
        }

        private readonly CreateExtendedDocFileDevTaskOptions _options;

        /// <inheritdoc/>
        protected override void OnValidateOptions()
        {
            ValidateOptionsAsNonNull(_options);
            ValidateFileName(nameof(_options.XmlDocFile), _options.XmlDocFile);
            ValidateFileName(nameof(_options.ExtendedDocFile), _options.ExtendedDocFile);
        }

        /// <inheritdoc/>
        protected override void OnRun()
        {
            var cSharpOriginDocLoaderOptions = new CSharpOriginDocLoaderOptions()
            {
                FileNamesList = new List<string>() { EVPath.Normalize(_options.XmlDocFile) },
                IgnoreErrors = true
            };

            var packageCardsList = CSharpXMLDocLoader.LoadOrigin(cSharpOriginDocLoaderOptions);

            var packageCard = packageCardsList.Single();

            JsonSerializationHelper.SerializeToFile(packageCard, _options.ExtendedDocFile);
        }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}Coverts standard documentation file `{_options.XmlDocFile}` to extended documentation file `{_options.ExtendedDocFile}`.");

            sb.Append(PrintValidation(n));

            return sb.ToString();
        }
    }
}
