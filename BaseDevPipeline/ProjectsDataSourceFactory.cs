using BaseDevPipeline.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public static class ProjectsDataSourceFactory
    {
        public static ProjectsDataSourceMode Mode { get; set; } = ProjectsDataSourceMode.Prod;

        public static ISymOntoClayProjectsSettings GetSymOntoClayProjectsSettings()
        {
            switch(Mode)
            {
                case ProjectsDataSourceMode.Prod:
                    {
                        var instance = new ProjectsDataSource();
                        return instance.GetSymOntoClayProjectsSettings();
                    }

                case ProjectsDataSourceMode.Test:
                    {
                        var instance = new TestProjectsDataSource();
                        return instance.GetSymOntoClayProjectsSettings();
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(Mode), Mode, null);
            }
        }

        public static ISolutionSettings GetSolution(KindOfProject kind)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetSolution(kind);
        }

        public static IReadOnlyList<ISolutionSettings> GetSolutions(KindOfProject kind)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetSolutions(kind);
        }

        public static IReadOnlyList<ISolutionSettings> GetSolutionsWithMaintainedReleases()
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetSolutionsWithMaintainedReleases();
        }

        public static IReadOnlyList<ISolutionSettings> GetSolutionsWithMaintainedVersionsInCSharpProjects()
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetSolutionsWithMaintainedVersionsInCSharpProjects();
        }

        public static IReadOnlyList<ISolutionSettings> GetSolutionsWhichUseCommonPakage()
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetSolutionsWhichUseCommonPakage();
        }

        public static IReadOnlyList<ISolutionSettings> GetUnityExampleSolutions()
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetUnityExampleSolutions();
        }

        public static IProjectSettings GetProject(KindOfProject kind)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetProject(kind);
        }

        public static IReadOnlyList<IProjectSettings> GetProjects(KindOfProject kind)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetProjects(kind);
        }

        public static IArtifactSettings GetDevArtifact(KindOfArtifact kind)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetDevArtifact(kind);
        }

        public static IReadOnlyList<IArtifactSettings> GetDevArtifacts(KindOfArtifact kind)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetDevArtifacts(kind);
        }

        public static ILicenseSettings GetLicense(string name)
        {
            var settings = GetSymOntoClayProjectsSettings();
            return settings.GetLicense(name);
        }
    }
}
