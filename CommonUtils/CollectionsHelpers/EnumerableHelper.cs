using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectionsHelpers.CollectionsHelpers
{
    public static class EnumerableHelper
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
    }
}
