using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class IniFile
    {
        public IniFile(string path)
        {
            Path = path;
        }

        public string Path { get; private set; }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public void WriteStringValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        public string ReadStringValue(string section, string key)
        {
            var temp = new StringBuilder(255);
            _ = GetPrivateProfileString(section, key, string.Empty, temp, 255, Path);
            return temp.ToString();
        }

        public void WriteBooleanValue(string section, string key, bool value)
        {
            if(value)
            {
                WriteStringValue(section, key, "true");
                return;
            }

            WriteStringValue(section, key, "false");
        }

        public bool? ReadBooleanValue(string section, string key)
        {
            var strVal = ReadStringValue(section, key);

            if(string.IsNullOrWhiteSpace(strVal))
            {
                return null;
            }

            if(strVal == "true")
            {
                return true;
            }

            if (strVal == "false")
            {
                return false;
            }

            return null;
        }

        public void WriteInt32Value(string section, string key, int value)
        {
            WriteStringValue(section, key, value.ToString());
        }

        public int? ReadInt32Value(string section, string key)
        {
            var strVal = ReadStringValue(section, key);

            if (string.IsNullOrWhiteSpace(strVal))
            {
                return null;
            }

            if(int.TryParse(strVal, out int result))
            {
                return result;
            }

            return null;
        }
    }
}
