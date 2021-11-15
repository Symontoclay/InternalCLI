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
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
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

        public static string GetOutputPath(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        {
            var project = LoadProject(projectFileName);

            var propertyGroup = GetPropertyGroup(project, kindOfConfiguration);

            var outputPathNode = propertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName.ToLower() == "OutputPath".ToLower());

            return (outputPathNode?.Value)?? string.Empty;
        }

        public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        {
            var project = LoadProject(projectFileName);

            var propertyGroup = GetPropertyGroup(project, kindOfConfiguration);

            var documentationFileNode = propertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName.ToLower() == "DocumentationFile".ToLower());

            return (documentationFileNode?.Value) ?? string.Empty;
        }

        public static bool SetDocumentationFileInUnityProjectIfEmpty(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        {
            return SetDocumentationFileIfEmpty(projectFileName, "Assembly-CSharp.xml", kindOfConfiguration);
        }

        public static bool SetDocumentationFileIfEmpty(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        {
            if(!string.IsNullOrWhiteSpace(GetDocumentationFile(projectFileName, kindOfConfiguration)))
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(Path.GetDirectoryName(documentationFileName)))
            {
                documentationFileName = Path.Combine(GetOutputPath(projectFileName, kindOfConfiguration), documentationFileName);
            }

            return SetDocumentationFile(projectFileName, documentationFileName, kindOfConfiguration);
        }

        public static bool SetDocumentationFile(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        {
            var project = LoadProject(projectFileName);

            var propertyGroup = GetPropertyGroup(project, kindOfConfiguration);

            var documentationFileNode = propertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName.ToLower() == "DocumentationFile".ToLower());

            if(documentationFileNode == null)
            {
                documentationFileNode = new XElement(XName.Get("DocumentationFile"));
                documentationFileNode.Value = documentationFileName;
                documentationFileNode.RemoveAttributes();
                propertyGroup.Add(documentationFileNode);
            }
            else
            {
                documentationFileNode.Value = documentationFileName;
            }

            SaveProject(project, projectFileName);

            return true;
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

            txt = txt.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", string.Empty).Replace("xmlns=\"\"", string.Empty).TrimStart();

            File.WriteAllText(projectFileName, txt);
        }

        private static XElement GetMainPropertyGroup(XElement project)
        {
            return project.Elements().FirstOrDefault(p => p.Name == "PropertyGroup" && !p.HasAttributes);
        }

        private static XElement GetPropertyGroup(XElement project, KindOfConfiguration kindOfConfiguration)
        {
            switch (kindOfConfiguration)
            {
                case KindOfConfiguration.Debug:
                    return GetDebugPropertyGroup(project);

                case KindOfConfiguration.Release:
                    return GetReleasePropertyGroup(project);

                default: 
                    throw new ArgumentOutOfRangeException(nameof(kindOfConfiguration), kindOfConfiguration, null);
            }
        }

        private static XElement GetDebugPropertyGroup(XElement project)
        {
            return project.Elements().FirstOrDefault(p => p.HasAttributes && p.Attributes().Any(x => x.Name == "Condition" && x.Value.Replace(" ", string.Empty).Trim() == "'$(Configuration)|$(Platform)'=='Debug|AnyCPU'"));
        }
        private static XElement GetReleasePropertyGroup(XElement project)
        {
            return project.Elements().FirstOrDefault(p => p.HasAttributes && p.Attributes().Any(x => x.Name == "Condition" && x.Value.Replace(" ", string.Empty).Trim() == "'$(Configuration)|$(Platform)'=='Release|AnyCPU'"));
        }
    }
}
