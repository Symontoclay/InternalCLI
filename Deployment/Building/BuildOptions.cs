﻿using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deployment.Building
{
    public class BuildOptions : IObjectToString
    {
        public List<BuildSourceSolutionOptions> SolutionsOptions { get; set; } = new List<BuildSourceSolutionOptions>();
        public List<BuildTargetOptions> TargetsOptions { get; set; } = new List<BuildTargetOptions>();

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

            sb.PrintObjListProp(n, nameof(SolutionsOptions), SolutionsOptions);
            sb.PrintObjListProp(n, nameof(TargetsOptions), TargetsOptions);

            return sb.ToString();
        }
    }
}
