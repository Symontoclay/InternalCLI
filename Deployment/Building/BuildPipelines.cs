using CommonUtils.DebugHelpers;
using CSharpUtils;
using Deployment.Building.Internal;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Deployment.Building
{
    public static class BuildPipelines
    {
#if DEBUG
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static void Run(BuildOptions options)
        {
#if DEBUG
            _logger.Info($"options = {options}");
#endif
            var kindOfBuild = KindOfBuild.Debug;

            var internalBuildSourceProjectOptionsList = ConvertToInternalBuildSourceProjectOptions(options.SolutionsOptions);

#if DEBUG
            _logger.Info($"internalBuildSourceProjectOptionsList = {internalBuildSourceProjectOptionsList.WriteListToString()}");
#endif
            var internalBuildSourceProjectOptionsDict = internalBuildSourceProjectOptionsList.GroupBy(p => p.Kind).ToDictionary(p => p.Key, p => p.ToList());

            foreach(var targetOptions in options.TargetsOptions)
            {
#if DEBUG
                _logger.Info($"targetOptions = {targetOptions}");
#endif
                var kind = targetOptions.Kind;

                switch(kind)
                {
                    case KindOfBuildTarget.NuGet:
                        ProcessNugetTarget(targetOptions, kindOfBuild, internalBuildSourceProjectOptionsDict);
                        break;

                    case KindOfBuildTarget.LibraryFolder:
                        ProcessLibraryFolderTarget(targetOptions, kindOfBuild, internalBuildSourceProjectOptionsDict);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }

#if DEBUG
            //_logger.Info($" = {}");
#endif
            //throw new NotImplementedException();

            _logger.Info("End");
        }

        private static void ProcessLibraryFolderTarget(BuildTargetOptions targetOptions, KindOfBuild kindOfBuild, Dictionary<KindOfSourceProject, List<InternalBuildSourceProjectOptions>> internalBuildSourceProjectOptionsDict)
        {
#if DEBUG
            _logger.Info($"targetOptions = {targetOptions}");
            _logger.Info($"kindOfBuild = {kindOfBuild}");
#endif

            var targetDir = targetOptions.TargetDir;

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            var librariesSourcesList = internalBuildSourceProjectOptionsDict[KindOfSourceProject.Library];

#if DEBUG
            _logger.Info($"librariesSourcesList = {librariesSourcesList.WriteListToString()}");
#endif

            foreach (var librarySource in librariesSourcesList)
            {
#if DEBUG
                _logger.Info($"librarySource = {librarySource}");
#endif
                if (librarySource.IsBuilt)
                {
                    CompleteProcessLibraryFolderTarget(targetOptions, librarySource);
                    continue;
                }

                BuildLibrary(librarySource, kindOfBuild);
#if DEBUG
                _logger.Info($"librarySource (after) = {librarySource}");
#endif
                CompleteProcessLibraryFolderTarget(targetOptions, librarySource);
            }
        }

        private static void CompleteProcessLibraryFolderTarget(BuildTargetOptions targetOptions, InternalBuildSourceProjectOptions internalBuildSourceProjectOptions)
        {
            foreach(var builtFileName in internalBuildSourceProjectOptions.BuiltFileNamesList)
            {
#if DEBUG
                _logger.Info($"builtFileName = {builtFileName}");
#endif

                var fileInfo = new FileInfo(builtFileName);

#if DEBUG
                _logger.Info($"fileInfo.Name = {fileInfo.Name}");
#endif

                var destFullFileName = Path.Combine(targetOptions.TargetDir, fileInfo.Name);

#if DEBUG
                _logger.Info($"destFullFileName = {destFullFileName}");
#endif

                File.Copy(builtFileName, destFullFileName, true);
            }
        }

        private static void ProcessNugetTarget(BuildTargetOptions targetOptions, KindOfBuild kindOfBuild, Dictionary<KindOfSourceProject, List<InternalBuildSourceProjectOptions>> internalBuildSourceProjectOptionsDict)
        {
#if DEBUG
            _logger.Info($"targetOptions = {targetOptions}");
            _logger.Info($"kindOfBuild = {kindOfBuild}");
#endif

            var targetDir = targetOptions.TargetDir;

            if(!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            var librariesSourcesList = internalBuildSourceProjectOptionsDict[KindOfSourceProject.Library];

#if DEBUG
            _logger.Info($"librariesSourcesList = {librariesSourcesList.WriteListToString()}");
#endif
            foreach (var librarySource in librariesSourcesList)
            {
#if DEBUG
                _logger.Info($"librarySource = {librarySource}");
#endif
                if (librarySource.IsBuilt)
                {
                    CompleteProcessNugetTarget(targetOptions, librarySource);
                    continue;
                }

                BuildLibrary(librarySource, kindOfBuild);
#if DEBUG
                _logger.Info($"librarySource (after) = {librarySource}");
#endif
                CompleteProcessNugetTarget(targetOptions, librarySource);
            }
        }

        private static void CompleteProcessNugetTarget(BuildTargetOptions targetOptions, InternalBuildSourceProjectOptions internalBuildSourceProjectOptions)
        {
            var fileInfo = new FileInfo(internalBuildSourceProjectOptions.NuGetFullFileName);

#if DEBUG
            _logger.Info($"fileInfo.Name = {fileInfo.Name}");
#endif

            var destFullFileName = Path.Combine(targetOptions.TargetDir, fileInfo.Name);

#if DEBUG
            _logger.Info($"destFullFileName = {destFullFileName}");
#endif

            File.Copy(internalBuildSourceProjectOptions.NuGetFullFileName, destFullFileName, true);
        }

        public static void BuildLibrary(InternalBuildSourceProjectOptions internalBuildSourceProjectOptions, KindOfBuild kindOfBuild)
        {
#if DEBUG
            _logger.Info($"internalBuildSourceProjectOptions = {internalBuildSourceProjectOptions}");
            _logger.Info($"kindOfBuild = {kindOfBuild}");
#endif

            var kindOfBuildResultDir = Path.Combine(internalBuildSourceProjectOptions.ProjectDir, "bin", GetKindOfBuildDirectoryFragment(kindOfBuild));

            _logger.Info($"kindOfBuildResultDir = {kindOfBuildResultDir}");

            var targetFramework = CSharpProjectHelper.GetTargetFramework(internalBuildSourceProjectOptions.ProjectFullFileName);

#if DEBUG
            _logger.Info($"targetFramework = {targetFramework}");
#endif

            var binFilesResultDir = Path.Combine(kindOfBuildResultDir, targetFramework);

#if DEBUG
            _logger.Info($"binFilesResultDir = {binFilesResultDir}");
#endif
            var process = Process.Start("dotnet", $"build {internalBuildSourceProjectOptions.ProjectFullFileName}");

            process.WaitForExit();

            var exitCode = process.ExitCode;

#if DEBUG
            _logger.Info($"process.ExitCode = {exitCode}");
#endif
            if (exitCode != 0)
            {
                throw new Exception($"Compilation of {internalBuildSourceProjectOptions.ProjectFullFileName} has been failed.");
            }

            var needGenerateNuGet = CSharpProjectHelper.GetGeneratePackageOnBuild(internalBuildSourceProjectOptions.ProjectFullFileName);

#if DEBUG
            _logger.Info($"needGenerateNuGet = {needGenerateNuGet}");
#endif
            if (needGenerateNuGet)
            {
                var nuGetFileName = Path.Combine(kindOfBuildResultDir, $"{GetNugetAssemblyName(internalBuildSourceProjectOptions.ProjectFullFileName)}.{GetNugetVersion(internalBuildSourceProjectOptions.ProjectFullFileName)}.nupkg");

#if DEBUG
                _logger.Info($"nuGetFileName = {nuGetFileName}");
#endif

                if(!File.Exists(nuGetFileName))
                {
                    throw new FileNotFoundException($"File {nuGetFileName} is not exist!");
                }

                internalBuildSourceProjectOptions.NuGetFullFileName = nuGetFileName;
            }

            var baseAssemblyName = GetBaseAssemblyName(internalBuildSourceProjectOptions.ProjectFullFileName);

#if DEBUG
            _logger.Info($"baseAssemblyName = {baseAssemblyName}");
#endif

            var targetBuiltFileNames = new List<string>() 
            {
                $"{baseAssemblyName}.dll",
                $"{baseAssemblyName}.pdb",
                $"{baseAssemblyName}.xml"
            };

            var builtFileNamesList = Directory.EnumerateFiles(binFilesResultDir).Where(p => targetBuiltFileNames.Any(x => p.EndsWith(x))).ToList();

#if DEBUG
            _logger.Info($"builtFileNamesList = {JsonConvert.SerializeObject(builtFileNamesList, Formatting.Indented)}");
#endif

            foreach (var builtFileName in builtFileNamesList)
            {
                if (!File.Exists(builtFileName))
                {
                    throw new FileNotFoundException($"File {builtFileName} is not exist!");
                }
            }

            internalBuildSourceProjectOptions.BuiltFileNamesList = builtFileNamesList;
            internalBuildSourceProjectOptions.IsBuilt = true;
        }

        private static string GetBaseAssemblyName(string projectFileName)
        {
#if DEBUG
            _logger.Info($"projectFileName = {projectFileName}");
#endif

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

#if DEBUG
            _logger.Info($"assemblyName = {assemblyName}");
#endif

            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                return assemblyName;
            }

            var fileInfo = new FileInfo(projectFileName);

#if DEBUG
            _logger.Info($"fileInfo.Name = {fileInfo.Name.Replace(".csproj", string.Empty)}");
#endif

            return fileInfo.Name.Replace(".csproj", string.Empty);
        }

        private static string GetNugetAssemblyName(string projectFileName)
        {
#if DEBUG
            _logger.Info($"projectFileName = {projectFileName}");
#endif

            var packageId = CSharpProjectHelper.GetPackageId(projectFileName);

#if DEBUG
            _logger.Info($"packageId = {packageId}");
#endif

            if(!string.IsNullOrWhiteSpace(packageId))
            {
                return packageId;
            }

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

#if DEBUG
            _logger.Info($"assemblyName = {assemblyName}");
#endif

            if (!string.IsNullOrWhiteSpace(assemblyName))
            {
                return assemblyName;
            }

            var fileInfo = new FileInfo(projectFileName);

#if DEBUG
            _logger.Info($"fileInfo.Name = {fileInfo.Name.Replace(".csproj", string.Empty)}");
#endif

            return fileInfo.Name.Replace(".csproj", string.Empty);
        }

        private static string GetNugetVersion(string projectFileName)
        {
#if DEBUG
            _logger.Info($"projectFileName = {projectFileName}");
#endif

            var initialVersion = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"initialVersion = {initialVersion}");
#endif

            var sb = new StringBuilder();

            var count = 0;

            foreach(var ch in initialVersion)
            {
                if(ch == '.')
                {
                    count++;
                }

                if(count == 3)
                {
                    return sb.ToString();
                }

                sb.Append(ch);
            }

            throw new NotImplementedException();
        }

        private static string GetKindOfBuildDirectoryFragment(KindOfBuild kindOfBuild)
        {
            switch(kindOfBuild)
            {
                case KindOfBuild.Debug:
                    return "Debug";

                case KindOfBuild.Release:
                    return "Release";

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfBuild), kindOfBuild, null);
            }
        }

        private static List<InternalBuildSourceProjectOptions> ConvertToInternalBuildSourceProjectOptions(List<BuildSourceSolutionOptions> solutionOptionsList)
        {
            var result = new List<InternalBuildSourceProjectOptions>();

            foreach (var solutionOptions in solutionOptionsList)
            {
#if DEBUG
                _logger.Info($"solutionOptions = {solutionOptions}");
#endif
                var solutionDir = solutionOptions.SolutionDir;

#if DEBUG
                _logger.Info($"solutionDir = {solutionDir}");
#endif

                foreach (var projectOptions in solutionOptions.ProjectsOptions)
                {
                    var item = new InternalBuildSourceProjectOptions();

                    var kind = projectOptions.Kind;

                    item.Kind = kind;

                    var projectDir = Path.Combine(solutionDir, projectOptions.ProjectDir);

                    if (!Directory.Exists(projectDir))
                    {
                        throw new DirectoryNotFoundException(projectDir);
                    }

                    item.ProjectDir = projectDir;

                    switch (kind)
                    {
                        case KindOfSourceProject.Library:
                        case KindOfSourceProject.CLI:
                        {
                                var projectFilesList = Directory.EnumerateFiles(projectDir).Where(p => p.EndsWith(".csproj")).ToList();

#if DEBUG
                                _logger.Info($"projectFilesList = {JsonConvert.SerializeObject(projectFilesList, Formatting.Indented)}");
#endif

                                switch(projectFilesList.Count)
                                {
                                    case 1:
                                        item.ProjectFullFileName = projectFilesList.SingleOrDefault();
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(projectFilesList.Count), projectFilesList.Count, null);
                                }
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                    }

                    result.Add(item);
                }
            }

            return result;
        }
    }
}
