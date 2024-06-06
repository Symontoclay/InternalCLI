using CommonUtils;
using CSharpUtils;
using NLog;
using NuGet.Frameworks;
using System;
using System.IO;

namespace TestSandBox
{
    public class CSharpProjectHelperTestsHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Run()
        {
            _logger.Info("Begin");

            //RunNetStandard();
            //RunNet();
            //RunNetFramework();
            RunNetWindows();

            _logger.Info("End");
        }

        private void RunNetStandard()
        {
            //RunNetStandard_GetSetTargetFramework();
            //RunNetStandard_GetSetTargetFrameworkVersion();
            //RunNetStandard_GetGeneratePackageOnBuild();
            RunNetStandard_GetAssemblyName();
        }

        private void RunNet()
        {
            //RunNet_GetSetTargetFramework();
            //RunNet_GetGeneratePackageOnBuild();
            RunNet_GetAssemblyName();
        }

        private void RunNetFramework()
        {
            //RunNetFramework_GetSetTargetFramework();
            //RunNetFramework_GetGeneratePackageOnBuild();
            RunNetFramework_GetAssemblyName();
        }

        private void RunNetWindows()
        {
            //RunNetWindows_GetSetTargetFramework();
            //RunNetWindows_GetGeneratePackageOnBuild();
            RunNetWindows_GetAssemblyName();
        }

        private void RunNetStandard_GetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(KindOfTargetCSharpFramework.NetStandard, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif

            CSharpProjectHelper.SetTargetFramework(projectFileName, "netstandard2.1");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif
        }

        private void RunNet_GetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(KindOfTargetCSharpFramework.Net, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif

            CSharpProjectHelper.SetTargetFramework(projectFileName, "net8.1");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif
        }

        private void RunNetFramework_GetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(KindOfTargetCSharpFramework.NetFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif

            CSharpProjectHelper.SetTargetFramework(projectFileName, "v4.7.2");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif
        }

        private void RunNetWindows_GetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(KindOfTargetCSharpFramework.NetWindows, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif

            CSharpProjectHelper.SetTargetFramework(projectFileName, "net8.0-windows");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif
        }

        private void RunNetStandard_GetSetTargetFrameworkVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif

            CSharpProjectHelper.SetTargetFramework(projectFileName, (KindOfTargetCSharpFramework.NetStandard, new Version(2, 1)));

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

#if DEBUG
            _logger.Info($"targetFrameworkVersion = '{targetFrameworkVersion}'");
#endif
        }

        private void RunNetStandard_GetGeneratePackageOnBuild()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var result = CSharpProjectHelper.GetGeneratePackageOnBuild(projectFileName);

#if DEBUG
            _logger.Info($"result = {result}");
#endif
        }

        private void RunNet_GetGeneratePackageOnBuild()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var result = CSharpProjectHelper.GetGeneratePackageOnBuild(projectFileName);

#if DEBUG
            _logger.Info($"result = {result}");
#endif
        }

        private void RunNetFramework_GetGeneratePackageOnBuild()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var result = CSharpProjectHelper.GetGeneratePackageOnBuild(projectFileName);

#if DEBUG
            _logger.Info($"result = {result}");
#endif
        }

        private void RunNetWindows_GetGeneratePackageOnBuild()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var result = CSharpProjectHelper.GetGeneratePackageOnBuild(projectFileName);

#if DEBUG
            _logger.Info($"result = {result}");
#endif
        }

        private void RunNetStandard_GetAssemblyName()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

#if DEBUG
            _logger.Info($"assemblyName = {assemblyName}");
#endif
        }

        private void RunNet_GetAssemblyName()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

#if DEBUG
            _logger.Info($"assemblyName = {assemblyName}");
#endif
        }

        private void RunNetFramework_GetAssemblyName()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

#if DEBUG
            _logger.Info($"assemblyName = {assemblyName}");
#endif
        }

        private void RunNetWindows_GetAssemblyName()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

#if DEBUG
            _logger.Info($"assemblyName = {assemblyName}");
#endif
        }

        /*
public static string GetPackageId(string projectFileName)

public static List<(string PackageId, Version Version)> GetInstalledPackages(string projectFileName)

public static string GetInstalledPackageVersion(string projectFileName, string packageId)
public static bool UpdateInstalledPackageVersion(string projectFileName, string packageId, string version)

public static string GetVersion(string projectFileName)
public static bool SetVersion(string projectFileName, string targetVersion)

public static bool GetCopyright(string projectFileName);
public static bool SetCopyright(string projectFileName, string copyright)

public static string GetOutputPath(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)

public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
public static bool SetDocumentationFileInUnityProjectIfEmpty(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
public static bool SetDocumentationFileIfEmpty(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
public static bool SetDocumentationFile(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/

        /*
        public static string GetTargetFramework(string projectFileName)
        public static bool SetTargetFramework(string projectFileName, string targetFramework)

        public static (KindOfTargetCSharpFramework Kind, Version Version) GetTargetFrameworkVersion(string projectFileName)
        public static bool SetTargetFramework(string projectFileName, (KindOfTargetCSharpFramework Kind, Version Version) frameworkVersion)

        public static bool GetGeneratePackageOnBuild(string projectFileName)
        public static string GetAssemblyName(string projectFileName)
        public static string GetPackageId(string projectFileName)

        public static List<(string PackageId, Version Version)> GetInstalledPackages(string projectFileName)

        public static string GetInstalledPackageVersion(string projectFileName, string packageId)
        public static bool UpdateInstalledPackageVersion(string projectFileName, string packageId, string version)

        public static string GetVersion(string projectFileName)
        public static bool SetVersion(string projectFileName, string targetVersion)

        public static bool SetCopyright(string projectFileName, string copyright)

        public static string GetOutputPath(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)

        public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        public static bool SetDocumentationFileInUnityProjectIfEmpty(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        public static bool SetDocumentationFileIfEmpty(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
        public static bool SetDocumentationFile(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
 */

        private string CreateTestCsProjectFile(KindOfTargetCSharpFramework kindOfTargetCSharpFramework, TempDirectory tempDirectory)
        {
            var csProjectSourceFileName = GetTestCsProjectFileSource(kindOfTargetCSharpFramework);

#if DEBUG
            _logger.Info($"csProjectSourceFileName = '{csProjectSourceFileName}'");
#endif

            var oldFullFileName = Path.Combine(Directory.GetCurrentDirectory(), "Projects", csProjectSourceFileName);

            var newFullFileName = Path.Combine(tempDirectory.FullName, csProjectSourceFileName.Replace(".xml", ".csproj"));

#if DEBUG
            _logger.Info($"oldFullFileName = '{oldFullFileName}'");
            _logger.Info($"newFullFileName = '{newFullFileName}'");
#endif

            File.Copy(oldFullFileName, newFullFileName, true);

            return newFullFileName;
        }

        private string GetTestCsProjectFileSource(KindOfTargetCSharpFramework kindOfTargetCSharpFramework)
        {
            switch(kindOfTargetCSharpFramework)
            {
                case KindOfTargetCSharpFramework.NetStandard:
                    return "NetStandard.xml";

                case KindOfTargetCSharpFramework.Net:
                    return "Net.xml";

                case KindOfTargetCSharpFramework.NetFramework:
                    return "NetFramework.xml";

                case KindOfTargetCSharpFramework.NetWindows:
                    return "NetWindows.xml";

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfTargetCSharpFramework), kindOfTargetCSharpFramework, null);
            }
        }
    }
}
