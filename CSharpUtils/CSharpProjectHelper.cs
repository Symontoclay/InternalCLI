using Newtonsoft.Json;
using NLog;
using SymOntoClay.Common.CollectionsHelpers;
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

            var section = GetTargetFrameworkSection(project);

            if(section == null)
            {
                section = GetTargetFrameworkSectionForNetFramework(project);

                if (section == null)
                {
                    return string.Empty;
                }
            }

            return section.Value;
        }

        private static XElement GetTargetFrameworkSection(XElement project)
        {
            var elementName = "TargetFramework";

            var targetPropertyGroup = GetMainPropertyGroup(project, elementName);

#if DEBUG
            //_logger.Info($"targetPropertyGroup = {targetPropertyGroup}");
#endif

            if(targetPropertyGroup == null)
            {
                return null;
            }

            return targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == elementName);
        }

        private static XElement GetTargetFrameworkSectionForNetFramework(XElement project)
        {
            var elementName = "TargetFrameworkVersion";

            var targetPropertyGroup = GetMainPropertyGroup(project, elementName);

#if DEBUG
            //_logger.Info($"targetPropertyGroup = {targetPropertyGroup}");
#endif

            if (targetPropertyGroup == null)
            {
                return null;
            }

            return targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == elementName);
        }

        public static (KindOfTargetCSharpFramework Kind, Version Version) GetTargetFrameworkVersion(string projectFileName)
        {
            return ConvertTargetFrameworkToVersion(GetTargetFramework(projectFileName));
        }

        private const string _netstandardPrefix = "netstandard";
        private const string _netPrefix = "net";
        private const string _netWindowsSuffix = "-windows";
        private const string _netFrameworkPrefix = "v";

        public static (KindOfTargetCSharpFramework Kind, Version Version) ConvertTargetFrameworkToVersion(string targetFramework)
        {
#if DEBUG
            //_logger.Info($"targetFramework = {targetFramework}");
#endif

            if (targetFramework.StartsWith(_netstandardPrefix))
            {
                var versionStr = targetFramework.Replace(_netstandardPrefix, string.Empty).Trim();

#if DEBUG
                //_logger.Info($"versionStr = {versionStr}");
#endif

                return (KindOfTargetCSharpFramework.NetStandard, new Version(versionStr));
            }

            if (targetFramework.StartsWith(_netPrefix))
            {
                var versionStr = targetFramework.Replace(_netPrefix, string.Empty).Trim();

#if DEBUG
                //_logger.Info($"versionStr = {versionStr}");
#endif

                if(targetFramework.EndsWith(_netWindowsSuffix))
                {
                    versionStr = versionStr.Replace(_netWindowsSuffix, string.Empty).Trim();

#if DEBUG
                    //_logger.Info($"versionStr (after) = {versionStr}");
#endif

                    return (KindOfTargetCSharpFramework.NetWindows, new Version(versionStr));
                }

                return (KindOfTargetCSharpFramework.Net, new Version(versionStr));
            }

            if(targetFramework.StartsWith(_netFrameworkPrefix))
            {
                var versionStr = targetFramework.Replace(_netFrameworkPrefix, string.Empty).Trim();

#if DEBUG
                //_logger.Info($"versionStr = {versionStr}");
#endif

                return (KindOfTargetCSharpFramework.NetFramework, new Version(versionStr));
            }

            throw new NotImplementedException();
        }

        public static string ConvertVersionToTargetFramework((KindOfTargetCSharpFramework Kind, Version Version) frameworkVersion)
        {
#if DEBUG
            //_logger.Info($"frameworkVersion = {frameworkVersion}");
#endif

            var kind = frameworkVersion.Kind;
            var version = frameworkVersion.Version;

            switch(kind)
            {
                case KindOfTargetCSharpFramework.NetStandard:
                    return _netstandardPrefix + version;

                case KindOfTargetCSharpFramework.Net:
                    return _netPrefix + version;

                case KindOfTargetCSharpFramework.NetFramework:
                    return _netFrameworkPrefix + version;

                case KindOfTargetCSharpFramework.NetWindows:
                    return _netPrefix + version + _netWindowsSuffix;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        public static bool SetTargetFramework(string projectFileName, string targetFramework)
        {
#if DEBUG
            //_logger.Info($"targetFramework = {targetFramework}");
#endif

            return SetTargetFramework(projectFileName, ConvertTargetFrameworkToVersion(targetFramework));
        }

        public static bool SetTargetFramework(string projectFileName, (KindOfTargetCSharpFramework Kind, Version Version) frameworkVersion)
        {
            var project = LoadProject(projectFileName);

#if DEBUG
            //_logger.Info($"frameworkVersion = {frameworkVersion}");
#endif

            var kind = frameworkVersion.Kind;
            var versionStr = ConvertVersionToTargetFramework(frameworkVersion);

#if DEBUG
            //_logger.Info($"versionStr = {versionStr}");
#endif

            var section = kind switch
            {
                KindOfTargetCSharpFramework.NetStandard => GetTargetFrameworkSection(project),
                KindOfTargetCSharpFramework.Net => GetTargetFrameworkSection(project),
                KindOfTargetCSharpFramework.NetFramework => GetTargetFrameworkSectionForNetFramework(project),
                KindOfTargetCSharpFramework.NetWindows => GetTargetFrameworkSection(project),
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };

            var existingVersion = section.Value;

            if (existingVersion != versionStr)
            {
                section.Value = versionStr;

                SaveProject(project, projectFileName);

                return true;
            }

            return false;
        }

        public static bool GetGeneratePackageOnBuild(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "GeneratePackageOnBuild");

            if(targetPropertyGroup == null)
            {
                return false;
            }

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "GeneratePackageOnBuild");

            if(section == null)
            {
                return false;
            }

            return bool.Parse(section.Value);
        }

        public static string GetAssemblyName(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "AssemblyName");

            if(targetPropertyGroup == null)
            {
                return string.Empty;
            }

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "AssemblyName");

            if (section == null)
            {
                return string.Empty;
            }

            return section.Value;
        }

        public static string GetPackageId(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "PackageId");

            if (targetPropertyGroup == null)
            {
                return string.Empty;
            }

            var section = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "PackageId");

            if (section == null)
            {
                return string.Empty;
            }

            return section.Value;
        }

        public static List<(string PackageId, Version Version)> GetInstalledPackages(string projectFileName)
        {
            var installedPackagesElementResult = GetInstalledPackagesElement(projectFileName);

            var itemGroup = installedPackagesElementResult.PackageElement;

#if DEBUG
            //_logger.Info($"itemGroup = {itemGroup}");
#endif

            if(itemGroup == null)
            {
                return new List<(string PackageId, Version Version)>();
            }

            return itemGroup.Elements().Select(p => (p.Attribute("Include").Value, new Version(p.Attribute("Version").Value.Replace("-Preview1", string.Empty).Trim()))).ToList();
        }

        public static string GetInstalledPackageVersion(string projectFileName, string packageId)
        {
            var packageItemResult = GetInstalledPackageElement(projectFileName, packageId);

            var packageItem = packageItemResult.PackageItem;

#if DEBUG
            //_logger.Info($"packageItem = {packageItem}");
#endif

            if(packageItem == null)
            {
                return string.Empty;
            }

            return packageItem.Attribute("Version").Value;
        }

        public static bool UpdateInstalledPackageVersion(string projectFileName, string packageId, string version)
        {
            var packageItemResult = GetInstalledPackageElement(projectFileName, packageId);

            var packageItem = packageItemResult.PackageItem;

#if DEBUG
            //_logger.Info($"packageItem = {packageItem}");
#endif

            if (packageItem == null)
            {
                return false;
            }

            var existingVersionStr = packageItem.Attribute("Version").Value;

            if (existingVersionStr != version)
            {
                packageItem.Attribute("Version").Value = version;

                SaveProject(packageItemResult.Project, projectFileName);

                return true;
            }

            return false;
        }

        private static (XElement PackageItem, XElement Project) GetInstalledPackageElement(string projectFileName, string packageId)
        {
            var installedPackagesElementResult = GetInstalledPackagesElement(projectFileName);

            var itemGroup = installedPackagesElementResult.PackageElement;
            var project = installedPackagesElementResult.Project;

            if (itemGroup == null)
            {
                return (null, project);
            }

            return (itemGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "PackageReference" && p.HasAttributes && p.Attributes().Any(x => x.Name.LocalName == "Include" && x.Value == packageId)), project);
        }

        private static (XElement PackageElement, XElement Project) GetInstalledPackagesElement(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var itemGroup = project.Elements().FirstOrDefault(p => p.Name.LocalName == "ItemGroup" && p.HasElements && p.Elements().Any(x => x.Name.LocalName == "PackageReference"));

#if DEBUG
            //_logger.Info($"itemGroup = {itemGroup}");
#endif

            return (itemGroup, project);
        }

        public static string GetVersion(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "Version");

            if(targetPropertyGroup == null)
            {
                return string.Empty;
            }

            var version = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "Version");

            if(version == null)
            {
                return string.Empty;
            }

            return version.Value;
        }

        public static string GetMaxVersionOfSolution(string solutionPath)
        {
            var projectsList = SolutionHelper.GetProjectsNames(solutionPath);

            if(projectsList.IsNullOrEmpty())
            {
                return null;
            }

#if DEBUG
            //_logger.Info($"projectsList.Count = {projectsList.Count}");
#endif

            var versionsList = new List<Version>();

            foreach(var project in projectsList)
            {
#if DEBUG
                //_logger.Info($"project = {project}");
#endif

                var versionStr = GetVersion(project);

#if DEBUG
                //_logger.Info($"versionStr = {versionStr}");
#endif

                if(string.IsNullOrEmpty(versionStr))
                {
                    continue;
                }

                versionsList.Add(new Version(versionStr));
            }

#if DEBUG
            //_logger.Info($"versionsList = {JsonConvert.SerializeObject(versionsList, Formatting.Indented)}");
#endif

            if(versionsList.Count == 0)
            {
                return null;
            }

            return versionsList.Max().ToString();
        }

        public static bool SetVersionToSolution(string solutionPath, string targetVersion)
        {
            var projectsList = SolutionHelper.GetProjectsNames(solutionPath);

            if (projectsList.IsNullOrEmpty())
            {
                return false;
            }

#if DEBUG
            //_logger.Info($"projectsList.Count = {projectsList.Count}");
#endif

            var result = false;

            foreach(var project in projectsList)
            {
#if DEBUG
                //_logger.Info($"project = {project}");
#endif

                if(SetVersion(project, targetVersion))
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool SetVersion(string projectFileName, string targetVersion)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "Version", true);

#if DEBUG
            //_logger.Info($"targetPropertyGroup = {targetPropertyGroup}");
#endif

            var needUpdate = false;

            var assemblyVersion = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "AssemblyVersion");

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

            var fileVersion = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "FileVersion");

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

            var version = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "Version");

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

        public static string GetCopyright(string projectFileName)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "Copyright");

#if DEBUG
            //_logger.Info($"targetPropertyGroup = {targetPropertyGroup}");
#endif

            if(targetPropertyGroup == null)
            {
                return string.Empty;
            }

            var copyrightSection = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "Copyright");

#if DEBUG
            //_logger.Info($"copyrightSection = {copyrightSection}");
#endif

            if (copyrightSection == null)
            {
                return string.Empty;
            }

            return copyrightSection.Value;
        }

        public static bool SetCopyright(string projectFileName, string copyright)
        {
            var project = LoadProject(projectFileName);

            var targetPropertyGroup = GetMainPropertyGroup(project, "Copyright", true);

            var needUpdate = false;

            var copyrightSection = targetPropertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName == "Copyright");

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

            if(propertyGroup == null)
            {
                return string.Empty;
            }

            var outputPathNode = propertyGroup.Elements().FirstOrDefault(p => p.Name.LocalName.ToLower() == "OutputPath".ToLower());

            return (outputPathNode?.Value)?? string.Empty;
        }

        public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        {
            var project = LoadProject(projectFileName);

            var propertyGroup = GetPropertyGroup(project, kindOfConfiguration);

            if (propertyGroup == null)
            {
                return string.Empty;
            }

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

            File.WriteAllText(projectFileName, txt, Encoding.UTF8);
        }

        private static XElement GetMainPropertyGroup(XElement project, string elementName, bool getFirstIfNotExists = false)
        {
#if DEBUG
            //_logger.Info($"project = {project}");
            //_logger.Info($"project.Elements().Select(p => p.Name) = {JsonConvert.SerializeObject(project.Elements().Select(p => (p.Name, p.HasAttributes, !p.HasAttributes, p.Name.LocalName == "PropertyGroup")), Formatting.Indented)}");
#endif

            var elements = project.Elements().Where(p => p.Name.LocalName == "PropertyGroup" && !p.HasAttributes);

            if(string.IsNullOrEmpty(elementName))
            {
                return elements.FirstOrDefault();
            }

            var result = elements.FirstOrDefault(p => p.Elements()?.Any(x => x.Name.LocalName == elementName) ?? false);

            if(result == null && getFirstIfNotExists)
            {
                return elements.FirstOrDefault(p => !(p.Elements()?.Any(x => x.Name.LocalName == "LangVersion") ?? false));
            }

            return result;
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
