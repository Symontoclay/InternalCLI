using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpUtils
{
    public static class CSharpFileHelper
    {
        public static bool AddCopyrightHeader(string fileName, string licenceText)
        {
            var content = File.ReadAllText(fileName);
            var startPos = content.IndexOf("/*");
            var needUpdate = false;

            if (startPos == -1)
            {
                needUpdate = true;
            }
            else
            {
                var stopPos = content.IndexOf("*/");
                var count = stopPos - startPos + 2;
                var bufferContent = content.Substring(0, startPos).Trim();

                if (bufferContent.Length == 0)
                {
                    var targetContent = content.Substring(startPos, count);

                    if (targetContent != licenceText)
                    {
                        content = content.Remove(startPos, count).TrimStart();
                        needUpdate = true;
                    }
                }
                else
                {
                    needUpdate = true;
                }
            }

            if (needUpdate)
            {
                var sb = new StringBuilder();
                sb.AppendLine(licenceText);
                sb.AppendLine();
                sb.Append(content);

                content = sb.ToString();

                File.WriteAllText(fileName, content);
            }

            return needUpdate;
        }
    }
}
