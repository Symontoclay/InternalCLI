using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public static class MD5Helper
    {
        public static string GetHash(string input)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}
