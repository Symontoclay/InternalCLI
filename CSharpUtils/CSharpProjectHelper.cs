using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CSharpUtils
{
    public static class CSharpProjectHelper
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static List<string> GetCSharpFileNames(string projectFileName)
        {
            var projFileInfo = new FileInfo(projectFileName);

            return BaseSolutionFilesHelper.GetFileNames(projFileInfo.DirectoryName, ".cs");
        }

        public static string GetTargetFramework(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "TargetFramework");

            if(section == null)
            {
                return string.Empty;
            }

            return section.Value;
        }

        public static bool GetGeneratePackageOnBuild(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "GeneratePackageOnBuild");

            if(section == null)
            {
                return false;
            }

            return bool.Parse(section.Value);
        }

        public static string GetAssemblyName(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "AssemblyName");

            if (section == null)
            {
                return string.Empty;
            }

            return section.Value;
        }

        public static string GetPackageId(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "PackageId");

            if (section == null)
            {
                return string.Empty;
            }

            return section.Value;
        }

        public static string GetVersion(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var version = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "Version");

            if(version == null)
            {
                return string.Empty;
            }

            return version.Value;
        }

        public static bool SetVersion(string projectFileName, string targetVersion)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var needUpdate = false;

            var assemblyVersion = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "AssemblyVersion");

            if (assemblyVersion == null)
            {
                assemblyVersion = new XElement("AssemblyVersion", targetVersion);
                targetPropertyGroup.Add(assemblyVersion);

                needUpdate = true;
            }
            else
            {
                if (assemblyVersion.Value != targetVersion)
                {
                    assemblyVersion.Value = targetVersion;

                    needUpdate = true;
                }
            }

            var fileVersion = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "FileVersion");

            if (fileVersion == null)
            {
                fileVersion = new XElement("FileVersion", targetVersion);
                targetPropertyGroup.Add(fileVersion);

                needUpdate = true;
            }
            else
            {
                if (fileVersion.Value != targetVersion)
                {
                    fileVersion.Value = targetVersion;

                    needUpdate = true;
                }
            }

            var version = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "Version");

            if (version == null)
            {
                version = new XElement("Version", targetVersion);
                targetPropertyGroup.Add(version);

                needUpdate = true;
            }
            else
            {
                if (version.Value != targetVersion)
                {
                    version.Value = targetVersion;

                    needUpdate = true;
                }
            }

            if (needUpdate)
            {
                SaveProject(project, projectFileName);
            }

            return needUpdate;
        }

        public static bool SetCopyright(string projectFileName, string copyright)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project);

            var needUpdate = false;

            var copyrightSection = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name == "Copyright");

            if (copyrightSection == null)
            {
                copyrightSection = new XElement("Copyright", copyright);
                targetPropertyGroup.Add(copyrightSection);

                needUpdate = true;
            }
            else
            {
                if (copyrightSection.Value != copyright)
                {
                    copyrightSection.Value = copyright;

                    needUpdate = true;
                }
            }

            if (needUpdate)
            {
                SaveProject(project, projectFileName);
            }

            return needUpdate;
        }

        private static XElement LoadProject(string projectFileName)
        {
            using (var fs = File.OpenRead(projectFileName))
            {
                return XElement.Load(fs);
            }
        }

        private static void SaveProject(XElement project, string projectFileName)
        {
            project.Save(projectFileName);
            var txt = File.ReadAllText(projectFileName);

            txt = txt.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", string.Empty).TrimStart();

            File.WriteAllText(projectFileName, txt);
        }

        private static XElement GetMainPropertyGroup(XElement project)
        {
            return project.Elements().FirstOrDefault(p => p.Name == "PropertyGroup" && !p.HasAttributes);
        }
    }
}
