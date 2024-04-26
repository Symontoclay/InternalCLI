using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.BuildReadme
{
    public class BuildReadmeTaskOptions : IObjectToString
    {
        public string SiteSourcePath { get; set; }
        public string SiteDestPath { get; set; }
        public string SiteName { get; set; }
        public string CommonBadgesFileName { get; set; }
        public string CommonReadmeFileName { get; set; }
        public string RepositorySpecificBadgesFileName { get; set; }
        public string RepositorySpecificReadmeFileName { get; set; }
        public string TargetReadmeFileName { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(SiteSourcePath)} = {SiteSourcePath}");
            sb.AppendLine($"{spaces}{nameof(SiteDestPath)} = {SiteDestPath}");
            sb.AppendLine($"{spaces}{nameof(SiteName)} = {SiteName}");
            sb.AppendLine($"{spaces}{nameof(CommonBadgesFileName)} = {CommonBadgesFileName}");
            sb.AppendLine($"{spaces}{nameof(CommonReadmeFileName)} = {CommonReadmeFileName}");
            sb.AppendLine($"{spaces}{nameof(RepositorySpecificBadgesFileName)} = {RepositorySpecificBadgesFileName}");
            sb.AppendLine($"{spaces}{nameof(RepositorySpecificReadmeFileName)} = {RepositorySpecificReadmeFileName}");
            sb.AppendLine($"{spaces}{nameof(TargetReadmeFileName)} = {TargetReadmeFileName}");

            return sb.ToString();
        }
    }
}
