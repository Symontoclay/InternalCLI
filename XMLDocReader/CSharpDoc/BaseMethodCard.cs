using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLDocReader.CSharpDoc
{
    public abstract class BaseMethodCard : MemberCard
    {
        public List<NamedElementCard> UsedTypesList { get; set; } = new List<NamedElementCard>();
        public List<MethodParamCard> ParamsList { get; set; } = new List<MethodParamCard>();

        /// <inheritdoc/>
        protected override string PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.PrintObjListProp(n, nameof(UsedTypesList), UsedTypesList);

            //sb.PrintObjListProp(n, nameof(TypeParamsList), TypeParamsList);
            sb.PrintObjListProp(n, nameof(ParamsList), ParamsList);

            sb.Append(base.PropertiesToString(n));
            return sb.ToString();
        }

        /// <inheritdoc/>
        protected override string PropertiesToShortString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            //sb.PrintShortObjListProp(n, nameof(TypeParamsList), TypeParamsList);
            sb.PrintShortObjListProp(n, nameof(ParamsList), ParamsList);

            sb.Append(base.PropertiesToShortString(n));
            return sb.ToString();
        }
    }
}
