﻿using SymOntoClay.Common;
using SymOntoClay.Common.DebugHelpers;
using System.Text;

namespace TestSandBox.RestoredDeploymentTasks
{
    public class TopSubItemNewDeploymentTaskOptions: IObjectToString
    {
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
            return sb.ToString();
        }
    }
}
