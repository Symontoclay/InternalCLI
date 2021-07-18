using CommonUtils.DebugHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteBuilder.SiteData
{
    public class MenuInfo : IObjectToString
    {
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();

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

            sb.PrintObjListProp(n, nameof(Items), Items);

            return sb.ToString();
        }

        public static MenuInfo GetMenu(string menuName, GeneralSiteBuilderSettings generalSiteBuilderSettings)
        {
            if (mMenuDict.ContainsKey(menuName))
            {
                return mMenuDict[menuName];
            }

            var menu = LoadFromFile(Path.Combine(generalSiteBuilderSettings.SourcePath, menuName));
            mMenuDict[menuName] = menu;
            return menu;
        }

        private static Dictionary<string, MenuInfo> mMenuDict = new Dictionary<string, MenuInfo>();

        public static MenuInfo LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<MenuInfo>(File.ReadAllText(path));
        }
    }
}
