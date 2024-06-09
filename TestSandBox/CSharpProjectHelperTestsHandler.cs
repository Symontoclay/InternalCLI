using CommonUtils;
using CSharpUtils;
using Newtonsoft.Json;
using NLog;
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
            //RunNetStandard_GetAssemblyName();
            //RunNetStandard_GetPackageId();
            //RunNetStandard_GetInstalledPackages();
            //RunNetStandard_GetUpdateInstalledPackageVersion();
            //RunNetStandard_GetSetVersion();
            //RunNetStandard_GetSetCopyright();
            //RunNetStandard_GetOutputPath();
            //RunNetStandard_GetSetDocumentationFileInUnityProjectIfEmpty();
            //RunNetStandard_GetSetDocumentationFileIfEmpty();
            RunNetStandard_GetSetDocumentationFile();
        }

        private void RunNet()
        {
            //RunNet_GetSetTargetFramework();
            //RunNet_GetGeneratePackageOnBuild();
            //RunNet_GetAssemblyName();
            //RunNet_GetPackageId();
            //RunNet_GetInstalledPackages();
            //RunNet_GetUpdateInstalledPackageVersion();
            //RunNet_GetSetVersion();
            //RunNet_GetSetCopyright();
            //RunNet_GetOutputPath();
            //RunNet_GetSetDocumentationFileInUnityProjectIfEmpty();
            //RunNet_GetSetDocumentationFileIfEmpty();
            RunNet_GetSetDocumentationFile();
        }

        private void RunNetFramework()
        {
            //RunNetFramework_GetSetTargetFramework();
            //RunNetFramework_GetGeneratePackageOnBuild();
            //RunNetFramework_GetAssemblyName();
            //RunNetFramework_GetPackageId();
            //RunNetFramework_GetInstalledPackages();
            //RunNetFramework_GetUpdateInstalledPackageVersion();
            //RunNetFramework_GetSetVersion();
            //RunNetFramework_GetSetCopyright();
            //RunNetFramework_GetOutputPath();
            //RunNetFramework_GetSetDocumentationFileInUnityProjectIfEmpty();
            //RunNetFramework_GetSetDocumentationFileIfEmpty();
            RunNetFramework_GetSetDocumentationFile();
        }

        private void RunNetWindows()
        {
            //RunNetWindows_GetSetTargetFramework();
            //RunNetWindows_GetGeneratePackageOnBuild();
            //RunNetWindows_GetAssemblyName();
            //RunNetWindows_GetPackageId();
            //RunNetWindows_GetInstalledPackages();
            //RunNetWindows_GetUpdateInstalledPackageVersion();
            //RunNetWindows_GetSetVersion();
            //RunNetWindows_GetSetCopyright();
            //RunNetWindows_GetOutputPath();
            //RunNetWindows_GetSetDocumentationFileInUnityProjectIfEmpty();
            //RunNetWindows_GetSetDocumentationFileIfEmpty();
            RunNetWindows_GetSetDocumentationFile();
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

        private void RunNetStandard_GetPackageId()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = CSharpProjectHelper.GetPackageId(projectFileName);

#if DEBUG
            _logger.Info($"packageId = {packageId}");
#endif
        }

        private void RunNet_GetPackageId()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = CSharpProjectHelper.GetPackageId(projectFileName);

#if DEBUG
            _logger.Info($"packageId = {packageId}");
#endif
        }

        private void RunNetFramework_GetPackageId()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = CSharpProjectHelper.GetPackageId(projectFileName);

#if DEBUG
            _logger.Info($"packageId = {packageId}");
#endif
        }

        private void RunNetWindows_GetPackageId()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = CSharpProjectHelper.GetPackageId(projectFileName);

#if DEBUG
            _logger.Info($"packageId = {packageId}");
#endif
        }

        private void RunNetStandard_GetInstalledPackages()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packagesList = CSharpProjectHelper.GetInstalledPackages(projectFileName);

#if DEBUG
            _logger.Info($"packagesList = {JsonConvert.SerializeObject(packagesList, Formatting.Indented)}");
#endif
        }

        private void RunNet_GetInstalledPackages()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packagesList = CSharpProjectHelper.GetInstalledPackages(projectFileName);

#if DEBUG
            _logger.Info($"packagesList = {JsonConvert.SerializeObject(packagesList, Formatting.Indented)}");
#endif
        }

        private void RunNetFramework_GetInstalledPackages()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packagesList = CSharpProjectHelper.GetInstalledPackages(projectFileName);

#if DEBUG
            _logger.Info($"packagesList = {JsonConvert.SerializeObject(packagesList, Formatting.Indented)}");
#endif
        }

        private void RunNetWindows_GetInstalledPackages()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packagesList = CSharpProjectHelper.GetInstalledPackages(projectFileName);

#if DEBUG
            _logger.Info($"packagesList = {JsonConvert.SerializeObject(packagesList, Formatting.Indented)}");
#endif
        }

        private void RunNetStandard_GetUpdateInstalledPackageVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = "NLog";

            var packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif

            var result = CSharpProjectHelper.UpdateInstalledPackageVersion(projectFileName, packageId, "5.1.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif
        }

        private void RunNet_GetUpdateInstalledPackageVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = "NLog";

            var packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif

            var result = CSharpProjectHelper.UpdateInstalledPackageVersion(projectFileName, packageId, "5.1.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif
        }

        private void RunNetFramework_GetUpdateInstalledPackageVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = "NLog";

            var packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif

            var result = CSharpProjectHelper.UpdateInstalledPackageVersion(projectFileName, packageId, "5.1.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif
        }

        private void RunNetWindows_GetUpdateInstalledPackageVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var packageId = "NLog";

            var packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif

            var result = CSharpProjectHelper.UpdateInstalledPackageVersion(projectFileName, packageId, "5.1.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

#if DEBUG
            _logger.Info($"packageVersion = '{packageVersion}'");
#endif
        }

        private void RunNetStandard_GetSetVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif

            var result = CSharpProjectHelper.SetVersion(projectFileName, "0.5.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif
        }

        private void RunNet_GetSetVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif

            var result = CSharpProjectHelper.SetVersion(projectFileName, "0.5.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif
        }

        private void RunNetFramework_GetSetVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif

            var result = CSharpProjectHelper.SetVersion(projectFileName, "0.5.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif
        }

        private void RunNetWindows_GetSetVersion()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif

            var result = CSharpProjectHelper.SetVersion(projectFileName, "0.5.5");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            version = CSharpProjectHelper.GetVersion(projectFileName);

#if DEBUG
            _logger.Info($"version = '{version}'");
#endif
        }

        private void RunNetStandard_GetSetCopyright()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif

            var result = CSharpProjectHelper.SetCopyright(projectFileName, "Copyright (c) Tst");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif
        }

        private void RunNet_GetSetCopyright()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif

            var result = CSharpProjectHelper.SetCopyright(projectFileName, "Copyright (c) Tst");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif
        }

        private void RunNetFramework_GetSetCopyright()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif

            var result = CSharpProjectHelper.SetCopyright(projectFileName, "Copyright (c) Tst");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif
        }

        private void RunNetWindows_GetSetCopyright()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif

            var result = CSharpProjectHelper.SetCopyright(projectFileName, "Copyright (c) Tst");

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            copyright = CSharpProjectHelper.GetCopyright(projectFileName);

#if DEBUG
            _logger.Info($"copyright = '{copyright}'");
#endif
        }

        private void RunNetStandard_GetOutputPath()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var outputPathDebug = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Debug);

#if DEBUG
            _logger.Info($"outputPathDebug = '{outputPathDebug}'");
#endif

            var outputPathRelease = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Release);

#if DEBUG
            _logger.Info($"outputPathRelease = '{outputPathRelease}'");
#endif
        }

        private void RunNet_GetOutputPath()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var outputPathDebug = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Debug);

#if DEBUG
            _logger.Info($"outputPathDebug = '{outputPathDebug}'");
#endif

            var outputPathRelease = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Release);

#if DEBUG
            _logger.Info($"outputPathRelease = '{outputPathRelease}'");
#endif
        }

        private void RunNetFramework_GetOutputPath()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var outputPathDebug = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Debug);

#if DEBUG
            _logger.Info($"outputPathDebug = '{outputPathDebug}'");
#endif

            var outputPathRelease = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Release);

#if DEBUG
            _logger.Info($"outputPathRelease = '{outputPathRelease}'");
#endif
        }

        private void RunNetWindows_GetOutputPath()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var outputPathDebug = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Debug);

#if DEBUG
            _logger.Info($"outputPathDebug = '{outputPathDebug}'");
#endif

            var outputPathRelease = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Release);

#if DEBUG
            _logger.Info($"outputPathRelease = '{outputPathRelease}'");
#endif
        }

        private void RunNetStandard_GetSetDocumentationFileInUnityProjectIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNet_GetSetDocumentationFileInUnityProjectIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetFramework_GetSetDocumentationFileInUnityProjectIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetWindows_GetSetDocumentationFileInUnityProjectIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetStandard_GetSetDocumentationFileIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNet_GetSetDocumentationFileIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetFramework_GetSetDocumentationFileIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetWindows_GetSetDocumentationFileIfEmpty()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetStandard_GetSetDocumentationFile()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFile(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNet_GetSetDocumentationFile()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFile(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetFramework_GetSetDocumentationFile()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFile(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        private void RunNetWindows_GetSetDocumentationFile()
        {
            var _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

#if DEBUG
            _logger.Info($"projectFileName = '{projectFileName}'");
#endif

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif

            var result = CSharpProjectHelper.SetDocumentationFile(projectFileName, "SomeDoc.xml", kindOfConfiguration);

#if DEBUG
            _logger.Info($"result = {result}");
#endif

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

#if DEBUG
            _logger.Info($"documentationFile = '{documentationFile}'");
#endif
        }

        /*
public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)

public static bool (string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
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
