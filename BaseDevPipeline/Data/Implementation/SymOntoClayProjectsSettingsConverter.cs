using BaseDevPipeline.SourceData;
using CommonUtils;
using NLog;
using SymOntoClay.Common;
using SymOntoClay.Common.CollectionsHelpers;
using SymOntoClay.Common.DebugHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BaseDevPipeline.Data.Implementation
{
    public static class SymOntoClayProjectsSettingsConverter
    {
#if DEBUG
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
#endif

        public static ISymOntoClayProjectsSettings Convert(SymOntoClaySettingsSource source, SymOntoClaySettingsSource modificationSource)
        {
            var result = new SymOntoClayProjectsSettings();
            result.BasePath = DetectBasePath(source.BasePaths);

            EVPath.RegVar("BASE_PATH", result.BasePath);

#if DEBUG
            _logger.Info($"source.Temp = {source.Temp}");
#endif

            var sourceTemp = source.Temp;
            var resultTemp = new TempSettings();
            result.Temp = resultTemp;

            if (sourceTemp != null)
            {
                if (!string.IsNullOrWhiteSpace(sourceTemp.Dir))
                {
                    resultTemp.Dir = PathsHelper.Normalize(sourceTemp.Dir);
                }

                resultTemp.ClearOnDispose = sourceTemp.ClearOnDispose;
            }

            result.SecretFilePath = DetectSecretFilePath(source.SecretsFilePaths);

            result.UtityExeInstances = DetectUnities();

            var artifactsForDeployment = source.ArtifactsForDeployment;

            if(artifactsForDeployment.IsNullOrEmpty())
            {
                result.ArtifactsForDeployment = new List<KindOfArtifact>();
            }
            else
            {
                result.ArtifactsForDeployment = artifactsForDeployment.Select(Enum.Parse<KindOfArtifact>).Distinct().ToList();
            }

            result.RepositoryReadmeSource = source.RepositoryReadmeSource;
            result.RepositoryBadgesSource = source.RepositoryBadgesSource;

            result.InternalCLIDist = PathsHelper.Normalize(source.InternalCLIDist);
            result.SocExePath = Path.Combine(result.InternalCLIDist, "soc.exe");

            result.Copyright = source.Copyright?.Replace("<curr_year/>", DateTime.Today.Year.ToString());

            var licensesDict = new Dictionary<string, LicenseSettings>();

            FillUpLicenses(source, result, licensesDict);
            FillUpDevArtifacts(source, result);
            FillUpSolutions(source, modificationSource, result, licensesDict);

            result.Prepare();

            return result;
        }

        private static string DetectBasePath(List<string> basePaths)
        {
            var normalizedBasePaths = basePaths.Select(PathsHelper.Normalize);

            var existingBasePaths = normalizedBasePaths.Where(Directory.Exists);

            var count = existingBasePaths.Count();

#if DEBUG
            //_logger.Info($"count = {count}");
            //_logger.Info($"existingBasePaths = {existingBasePaths.WritePODListToString()}");
#endif

            switch (count)
            {
                case 0:
                    return string.Empty;

                case 1:
                    return existingBasePaths.Single();

                default:
                    return existingBasePaths.First();
            }
        }

        private static string DetectSecretFilePath(List<string> secretsFilePaths)
        {
            var normalizedSecretsFilePaths = secretsFilePaths.Select(PathsHelper.Normalize);

            var existingSecretsFilePaths = normalizedSecretsFilePaths.Where(File.Exists);

            var count = existingSecretsFilePaths.Count();

#if DEBUG
            //_logger.Info($"count = {count}");
            //_logger.Info($"existingSecretsFilePaths = {existingSecretsFilePaths.WritePODListToString()}");
#endif

            switch (count)
            {
                case 0:
                    return string.Empty;

                case 1:
                    return existingSecretsFilePaths.Single();

                default:
                    return existingSecretsFilePaths.First();
            }
        }

        private static List<UtityExeInstance> DetectUnities()
        {
            var baseUnityPath = EVPath.Normalize("%ProgramFiles%/Unity/Hub/Editor");

            if(!Directory.Exists(baseUnityPath))
            {
                return new List<UtityExeInstance>();
            }

            var existingUnityPaths = Directory.GetDirectories(baseUnityPath);

            if (!existingUnityPaths.Any())
            {
                return new List<UtityExeInstance>();
            }

            var result = new List<UtityExeInstance>();

            foreach(var initUnityPath in existingUnityPaths)
            {
                var unityPath = Path.Combine(initUnityPath, "Editor", "Unity.exe");

                try
                {
                    using (var process = new ProcessSyncWrapper(unityPath, "-version"))
                    {
                        var exitCode = process.Run();

                        if (exitCode != 0)
                        {
                            continue;
                        }

                        var output = process.Output.SingleOrDefault();

                        var item = new UtityExeInstance()
                        {
                            Path = unityPath,
                            Version = output
                        };

                        result.Add(item);
                    }
                }
                catch
                {
                }
            }

            return result;
        }

        private static void FillUpSolutions(SymOntoClaySettingsSource source, SymOntoClaySettingsSource modificationSource, SymOntoClayProjectsSettings result, Dictionary<string, LicenseSettings> licensesDict)
        {
            var soulutions = new List<SolutionSettings>();
            result.Solutions = soulutions;

            var modificationSolutionsDict = modificationSource?.Solutions?.ToDictionary(p => p.Name, p => p);

            foreach(var solutionSource in source.Solutions)
            {
                var name = solutionSource.Name;

#if DEBUG
                //_logger.Info($"name = {name}");
#endif

                var modificationSolution = modificationSolutionsDict == null ? null : (modificationSolutionsDict.ContainsKey(name) ? modificationSolutionsDict[name] : null);

#if DEBUG
                //_logger.Info($"modificationSolution = {modificationSolution}");
#endif

                var item = new SolutionSettings();
                item.Name = name;
                item.Kind = Enum.Parse<KindOfProject>(solutionSource.Kind);

                var href = string.IsNullOrEmpty(modificationSolution?.Href) ? solutionSource.Href : modificationSolution.Href;

                item.Href = href;

                if(!string.IsNullOrWhiteSpace(href))
                {
                    item.GitFileHref = solutionSource.GitFileHref;

                    var uri = new Uri(href);

                    var hrefPath = uri.PathAndQuery.Substring(1);

                    var slashPos = hrefPath.IndexOf("/");

                    item.OwnerName = hrefPath.Substring(0, slashPos);

                    item.RepositoryName = hrefPath.Substring(slashPos + 1);
                }

                item.Path = string.IsNullOrEmpty(modificationSolution?.Path) ? PathsHelper.Normalize(solutionSource.Path) : PathsHelper.Normalize(modificationSolution.Path);

                EVPath.RegVar("SLN_ROOT_PATH", item.Path);

                item.SlnPath = DetectSlnPath(solutionSource.SlnPath, item.Path);

                if(string.IsNullOrWhiteSpace(item.SlnPath) && (item.Kind == KindOfProject.Unity || item.Kind == KindOfProject.UnityExample))
                {
                    var dirInfo = new DirectoryInfo(item.Path);

                    item.SlnPath = Path.Combine(item.Path, $"{dirInfo.Name}.sln");
                }

                item.SourcePath = PathsHelper.Normalize(solutionSource.SourcePath);

                if(string.IsNullOrWhiteSpace(item.SourcePath))
                {
                    item.SourcePath = item.Path;
                }

                item.BuiltNuGetPackages = solutionSource.BuiltNuGetPackages;

                if (string.IsNullOrWhiteSpace(solutionSource.License))
                {
                    item.LicenseName = string.Empty;
                }
                else
                {
                    if(licensesDict.ContainsKey(solutionSource.License))
                    {
                        item.LicenseName = solutionSource.License;
                        item.License = licensesDict[solutionSource.License];
                    }
                    else
                    {
                        throw new Exception($"The license '{solutionSource.License}' hasn't been described.");
                    }                    
                }

                if (solutionSource.Projects.IsNullOrEmpty())
                {
                    if(item.Kind == KindOfProject.Unity)
                    {
                        InitProjectsList(item, result);

                        var projectSource = new ProjectSource()
                        {
                            Kind = "Unity",
                            CsProjPath = "%SLN_ROOT_PATH%/Assembly-CSharp.csproj"
                        };

#if DEBUG
                        //_logger.Info($"projectSource = {projectSource}");
#endif

                        FillUpProject(projectSource, item, result);
                    }
                }
                else
                {
                    FillUpProjects(item, solutionSource, result);
                }

                var artifactsForDeployment = solutionSource.ArtifactsForDeployment;

                if(artifactsForDeployment.IsNullOrEmpty())
                {
                    item.ArtifactsForDeployment = result.ArtifactsForDeployment.ToList();
                }
                else
                {
                    if(!artifactsForDeployment.Any(p => p == "no artifacts"))
                    {
                        var solutionsArtifactsForDeployment = artifactsForDeployment.Where(p => p != "inherited").Select(p => Enum.Parse<KindOfArtifact>(p)).Distinct().ToList();

                        if(artifactsForDeployment.Any(p => p == "inherited"))
                        {
                            item.ArtifactsForDeployment = result.ArtifactsForDeployment.Concat(solutionsArtifactsForDeployment).Distinct().ToList();
                        }
                        else
                        {
                            item.ArtifactsForDeployment = solutionsArtifactsForDeployment;
                        }
                    }
                }

                item.EnableGenerateReadme = solutionSource.EnableGenerateReadme;

                if (string.IsNullOrWhiteSpace(solutionSource.BadgesSource))
                {
                    item.BadgesSource = PathsHelper.Normalize(result.RepositoryBadgesSource);
                }
                else
                {
                    item.BadgesSource = PathsHelper.Normalize(solutionSource.BadgesSource);
                }

                if(string.IsNullOrWhiteSpace(solutionSource.ReadmeSource))
                {
                    item.ReadmeSource = PathsHelper.Normalize(result.RepositoryReadmeSource);
                }
                else
                {
                    item.ReadmeSource = PathsHelper.Normalize(solutionSource.ReadmeSource);
                }

                item.IsCommonReadmeSource = solutionSource.IsCommonReadmeSource;
                item.CommonReadmeSource = PathsHelper.Normalize(solutionSource.CommonReadmeSource);
                item.CommonBadgesSource = PathsHelper.Normalize(solutionSource.CommonBadgesSource);

                item.CodeOfConductSource = PathsHelper.Normalize(solutionSource.CodeOfConductSource);
                item.ContributingSource = PathsHelper.Normalize(solutionSource.ContributingSource);

                item.RereadUnityVersion();

                soulutions.Add(item);
            }
        }

        private static string DetectSlnPath(string sourceSlnPath, string path)
        {
            if (string.IsNullOrWhiteSpace(sourceSlnPath))
            {
                if(!Directory.Exists(path))
                {
                    return string.Empty;
                }

#if DEBUG
                //_logger.Info($"sourceSlnPath = {sourceSlnPath}");
                //_logger.Info($"path = {path}");
#endif

                var slnFiles = Directory.GetFiles(path, "*.sln").Select(p => PathsHelper.Normalize(p));

#if DEBUG
                //_logger.Info($"slnFiles = {JsonConvert.SerializeObject(slnFiles, Formatting.Indented)}");
#endif

                if (!slnFiles.Any())
                {
                    return string.Empty;
                }

                if(slnFiles.Count() == 1)
                {
                    return slnFiles.Single();
                }

                if(slnFiles.Count() == 2)
                {
                    return slnFiles.First();
                }

                throw new NotImplementedException();
            }

            return PathsHelper.Normalize(sourceSlnPath);
        }

        private static void InitProjectsList(SolutionSettings solution, SymOntoClayProjectsSettings result)
        {
            solution.Projects = new List<ProjectSettings>();

            if (result.Projects == null)
            {
                result.Projects = new List<ProjectSettings>();
            }
        }

        private static void FillUpProjects(SolutionSettings solution, SolutionSource soutionSource, SymOntoClayProjectsSettings result)
        {
            InitProjectsList(solution, result);

            foreach(var projectSource in soutionSource.Projects)
            {
                FillUpProject(projectSource, solution, result);
            }
        }

        private static void FillUpProject(ProjectSource projectSource, SolutionSettings solution, SymOntoClayProjectsSettings result)
        {
            var item = new ProjectSettings()
            {
                Solution = solution,
                LicenseName = solution.LicenseName,
                License = solution.License
            };

            item.Kind = Enum.Parse<KindOfProject>(projectSource.Kind);

            item.FolderName = projectSource.Path;

            if(!string.IsNullOrWhiteSpace(projectSource.Path))
            {
                if (projectSource.Path.StartsWith("%") || projectSource.Path.Contains(":"))
                {
                    item.Path = PathsHelper.Normalize(projectSource.Path);
                }
                else
                {
                    item.Path = Path.Combine(solution.Path, projectSource.Path);
                }
            }

            item.CsProjPath = DetectCsProjPath(projectSource.CsProjPath, item.Path);

            solution.Projects.Add(item);
            result.Projects.Add(item);
        }

        private static string DetectCsProjPath(string sourceCsProjPath, string path)
        {
            if (string.IsNullOrWhiteSpace(sourceCsProjPath))
            {
                var csProjFiles = Directory.GetFiles(path, "*.csproj").Select(p => PathsHelper.Normalize(p));

                if (!csProjFiles.Any())
                {
                    return string.Empty;
                }

                if (csProjFiles.Count() == 1)
                {
                    return csProjFiles.Single();
                }

                throw new NotImplementedException();
            }

            return PathsHelper.Normalize(sourceCsProjPath);
        }

        private static void FillUpDevArtifacts(SymOntoClaySettingsSource source, SymOntoClayProjectsSettings result)
        {
            var artifacts = new List<ArtifactSettings>();

            result.DevArtifacts = artifacts;

            foreach(var artifactDest in source.DevArtifacts)
            {
                var item = new ArtifactSettings();

                item.Kind = Enum.Parse<KindOfArtifact>(artifactDest.Kind);

                item.Path = PathsHelper.Normalize(artifactDest.Path);

                artifacts.Add(item);
            }
        }

        private static void FillUpLicenses(SymOntoClaySettingsSource source, SymOntoClayProjectsSettings result, Dictionary<string, LicenseSettings> licensesDict)
        {
            var licenses = new List<LicenseSettings>();

            result.Licenses = licenses;

            foreach (var licenseSource in source.Licenses)
            {
                var name = licenseSource.Name;

                if(string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception("License's name can not be null or empty.");
                }

                if(licensesDict.ContainsKey(name))
                {
                    throw new Exception($"The license '{name}' is duplicated.");
                }

                var item = new LicenseSettings
                {
                    Name = name,
                    HeaderFileName = PathsHelper.Normalize(licenseSource.HeaderFileName),
                    HeaderContent = licenseSource.HeaderContent,
                    FileName = PathsHelper.Normalize(licenseSource.FileName),
                    Content = licenseSource.Content
                };

                if(!string.IsNullOrWhiteSpace(item.FileName) && !File.Exists(item.FileName))
                {
                    throw new Exception($"The license's file '{item.FileName}' doesn't exist.");
                }

                if (!string.IsNullOrWhiteSpace(item.HeaderFileName) && !File.Exists(item.HeaderFileName))
                {
                    throw new Exception($"The license's file '{item.HeaderFileName}' doesn't exist.");
                }

                if (string.IsNullOrWhiteSpace(item.Content) && !string.IsNullOrWhiteSpace(item.FileName))
                {
                    item.Content = PrepareLicenseContent(File.ReadAllText(item.FileName));
                }

                if (string.IsNullOrWhiteSpace(item.HeaderContent) && !string.IsNullOrWhiteSpace(item.HeaderFileName))
                {
                    item.HeaderContent = PrepareLicenseContent(File.ReadAllText(item.HeaderFileName));
                }

                if (string.IsNullOrWhiteSpace(item.HeaderFileName) && !string.IsNullOrWhiteSpace(item.FileName))
                {
                    item.HeaderFileName = item.FileName;
                }

                if (string.IsNullOrWhiteSpace(item.HeaderContent) && !string.IsNullOrWhiteSpace(item.Content))
                {
                    item.HeaderContent = item.Content;
                }

                licenses.Add(item);

                licensesDict[name] = item;
            }
        }

        private static string PrepareLicenseContent(string source)
        {
            return source;
        }
    }
}
