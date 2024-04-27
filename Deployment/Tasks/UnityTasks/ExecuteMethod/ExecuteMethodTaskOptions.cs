﻿using CommonUtils.DebugHelpers;
using SymOntoClay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Tasks.UnityTasks.ExecuteMethod
{
    public class ExecuteMethodTaskOptions : IObjectToString
    {
        public string UnityExeFilePath { get; set; }
        public string RootDir { get; set; }
        public string MethodName { get; set; }

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

            sb.AppendLine($"{spaces}{nameof(UnityExeFilePath)} = {UnityExeFilePath}");
            sb.AppendLine($"{spaces}{nameof(RootDir)} = {RootDir}");
            sb.AppendLine($"{spaces}{nameof(MethodName)} = {MethodName}");

            return sb.ToString();
        }
    }
}
