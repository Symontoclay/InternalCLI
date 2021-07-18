using BaseDevPipeline.SourceData;
using CollectionsHelpers.CollectionsHelpers;
using CommonUtils;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public static class SymOntoClayProjectsSettingsConverter
    {
        public static ISymOntoClayProjectsSettings Convert(SymOntoClaySettingsSource source)
        {
            var result = new SymOntoClayProjectsSettings();
            result.BasePath = DetectBasePath(source.BasePaths);

            EVPath.RegVar("BASE_PATH", result.BasePath);

            result.SecretFilePath = DetectSecretFilePath(source.SecretsFilePaths);

            result.UtityExeInstances = DetectUnities(source.UnityPaths);

            var licensesDict = new Dictionary<string, LicenseSettings>();

            FillUpLicenses(source, result, licensesDict);
            FillUpArtifacts(source, result);
            FillUpSolutions(source, result, licensesDict);

            result.Prepare();

            return result;
        }

        private static string DetectBasePath(List<string> basePaths)
        {
            var normalizedBasePaths = basePaths.Select(p => PathsHelper.Normalize(p));

            var existingBasePaths = normalizedBasePaths.Where(p => Directory.Exists(p));

            var count = existingBasePaths.Count();

            switch (count)
            {
                case 1:
                    return existingBasePaths.Single();
            }

            throw new NotImplementedException();
        }

        private static string DetectSecretFilePath(List<string> secretsFilePaths)
        {
            var normalizedSecretsFilePaths = secretsFilePaths.Select(p => PathsHelper.Normalize(p));

            var existingSecretsFilePaths = normalizedSecretsFilePaths.Where(p => File.Exists(p));

            var count = existingSecretsFilePaths.Count();

            switch (count)
            {
                case 1:
                    return existingSecretsFilePaths.Single();
            }

            throw new NotImplementedException();
        }

        private static List<UtityExeInstance> DetectUnities(List<string> unityPaths)
        {
            var normalizedUnityPaths = unityPaths.Select(p => PathsHelper.Normalize(p));

            var existingUnityPaths = normalizedUnityPaths.Where(p => File.Exists(p));

            if(!existingUnityPaths.Any())
            {
                return new List<UtityExeInstance>();
            }

            var result = new List<UtityExeInstance>();

            foreach(var utityPath in existingUnityPaths)
            {
                try
                {
                    using (var process = new ProcessSyncWrapper(utityPath, "-version"))
                    {
                        var exitCode = process.Run();

                        if (exitCode != 0)
                        {
                            continue;
                        }

                        var output = process.Output.SingleOrDefault();

                        var item = new UtityExeInstance()
                        {
                            Path = utityPath,
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

        private static void FillUpSolutions(SymOntoClaySettingsSource source, SymOntoClayProjectsSettings result, Dictionary<string, LicenseSettings> licensesDict)
        {
            var soulutions = new List<SolutionSettings>();
            result.Solutions = soulutions;

            foreach(var soutionSource in source.Solutions)
            {
                var item = new SolutionSettings();
                item.Kind = Enum.Parse<KindOfProject>(soutionSource.Kind);
                item.Path = PathsHelper.Normalize(soutionSource.Path);

                EVPath.RegVar("SLN_ROOT_PATH", item.Path);

                item.SlnPath = DetectSlnPath(soutionSource.SlnPath, item.Path);

                item.SourcePath = PathsHelper.Normalize(soutionSource.SourcePath);

                if(string.IsNullOrWhiteSpace(soutionSource.License))
                {
                    item.LicenseName = string.Empty;
                }
                else
                {
                    if(licensesDict.ContainsKey(soutionSource.License))
                    {
                        item.LicenseName = soutionSource.License;
                        item.License = licensesDict[soutionSource.License];
                    }
                    else
                    {
                        throw new Exception($"The license '{soutionSource.License}' hasn't been described.");
                    }                    
                }

                if (!soutionSource.Projects.IsNullOrEmpty())
                {
                    FillUpProjects(item, soutionSource, result);
                }

                soulutions.Add(item);
            }
        }

        private static string DetectSlnPath(string sourceSlnPath, string path)
        {
            if (string.IsNullOrWhiteSpace(sourceSlnPath))
            {
                var slnFiles = Directory.GetFiles(path, "*.sln").Select(p => PathsHelper.Normalize(p));

                if(!slnFiles.Any())
                {
                    return string.Empty;
                }

                if(slnFiles.Count() == 1)
                {
                    return slnFiles.Single();
                }

                throw new NotImplementedException();
            }

            return PathsHelper.Normalize(sourceSlnPath);
        }

        private static void FillUpProjects(SolutionSettings solution, SolutionSource soutionSource, SymOntoClayProjectsSettings result)
        {
            var solutionProjects = new List<ProjectSettings>();
            solution.Projects = solutionProjects;

            if(result.Projects == null)
            {
                result.Projects = new List<ProjectSettings>();
            }

            foreach(var projectSource in soutionSource.Projects)
            {
                var item = new ProjectSettings()
                {
                    Solution = solution,
                    LicenseName = solution.LicenseName,
                    License = solution.License
                };

                item.Kind = Enum.Parse<KindOfProject>(projectSource.Kind);

                if(projectSource.Path.StartsWith("%") || projectSource.Path.Contains(":"))
                {
                    item.Path = PathsHelper.Normalize(projectSource.Path);
                }
                else
                {
                    item.Path = Path.Combine(solution.Path, projectSource.Path);
                }                

                item.CsProjPath = DetectCsProjPath(projectSource.CsProjPath, item.Path);

                solutionProjects.Add(item);
                result.Projects.Add(item);
            }
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

        private static void FillUpArtifacts(SymOntoClaySettingsSource source, SymOntoClayProjectsSettings result)
        {
            var artifacts = new List<ArtifactSettings>();

            result.Artifacts = artifacts;

            foreach(var artifactDest in source.Artifacts)
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
