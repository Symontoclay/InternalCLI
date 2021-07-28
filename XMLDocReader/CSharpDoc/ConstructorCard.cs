using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XMLDocReader.CSharpDoc
{
    public class ConstructorCard : BaseMethodCard
    {
        public ConstructorInfo ConstructorInfo { get; set; }

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(ConstructorInfo), ConstructorInfo);

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintExisting(n, nameof(ConstructorInfo), ConstructorInfo);

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
