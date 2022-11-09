using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public static class Base64Helper
    {
        public static string GetBase64String(string input)
        {
            var base64Array = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(base64Array);
        }
    }
}
