using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System.Collections.Generic;
using System.Text;

namespace Deployment.DevTasks.RemoveSingleLineComments
{
    public class RemoveSingleLineCommentsDevTaskOptions : IObjectToString
    {
        public List<string> TargetDirsList { get; set; }
        
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

            sb.PrintPODList(n, nameof(TargetDirsList), TargetDirsList);

            return sb.ToString();
        }
    }
}
