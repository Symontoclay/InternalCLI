﻿using CommonUtils;
using CSharpUtils;
using System.Reflection;

namespace CSharpUtilsTests
{
    public class CSharpProjectHelperNetTests: BaseCSharpProjectHelperTests
    {
        private KindOfTargetCSharpFramework _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.Net;

        [Test]
        public void TestGetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("net8.0"));

            CSharpProjectHelper.SetTargetFramework(projectFileName, "net8.1");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("net8.1"));
        }

        [Test]
        public void TestGetSetTargetFrameworkVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.Net, new Version(8, 0))));

            CSharpProjectHelper.SetTargetFramework(projectFileName, (KindOfTargetCSharpFramework.Net, new Version(8, 1)));

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.Net, new Version(8, 1))));
        }

        [Test]
        public void TestGetGeneratePackageOnBuild()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var result = CSharpProjectHelper.GetGeneratePackageOnBuild(projectFileName);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void TestGetAssemblyName()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

            Assert.That(assemblyName, Is.EqualTo(""));
        }

        [Test]
        public void TestGetPackageId()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var packageId = CSharpProjectHelper.GetPackageId(projectFileName);

            Assert.That(packageId, Is.EqualTo(""));
        }

        [Test]
        public void TestGetInstalledPackages()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var packagesList = CSharpProjectHelper.GetInstalledPackages(projectFileName);

            Assert.That(packagesList.Count, Is.EqualTo(4));

            var item1 = packagesList[0];

            Assert.That(item1.PackageId, Is.EqualTo("Newtonsoft.Json"));
            Assert.That(item1.Version, Is.EqualTo(new Version("13.0.3")));

            var item2 = packagesList[1];

            Assert.That(item2.PackageId, Is.EqualTo("NLog"));
            Assert.That(item2.Version, Is.EqualTo(new Version("5.1.4")));

            var item3 = packagesList[2];

            Assert.That(item3.PackageId, Is.EqualTo("System.Configuration.ConfigurationManager"));
            Assert.That(item3.Version, Is.EqualTo(new Version("4.7.0")));

            var item4 = packagesList[3];

            Assert.That(item4.PackageId, Is.EqualTo("SymOntoClay.Common"));
            Assert.That(item4.Version, Is.EqualTo(new Version("0.5.4.9")));
        }

        [Test]
        public void TestGetUpdateInstalledPackageVersion()
        {
            throw new NotImplementedException();

            /*
            public static string GetInstalledPackageVersion(string projectFileName, string packageId)
public static bool UpdateInstalledPackageVersion(string projectFileName, string packageId, string version)
*/
        }

        [Test]
        public void TestGetSetVersion()
        {
            throw new NotImplementedException();

            /*
            public static string GetVersion(string projectFileName)
public static bool SetVersion(string projectFileName, string targetVersion)
*/
        }

        [Test]
        public void TestGetSetCopyright()
        {
            throw new NotImplementedException();

            /*
            public static bool SetCopyright(string projectFileName, string copyright)
*/
        }

        [Test]
        public void TestGetOutputPathDebug()
        {
            throw new NotImplementedException();

            /*
            public static string GetOutputPath(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetOutputPathRelease()
        {
            throw new NotImplementedException();

            /*
            public static string GetOutputPath(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetSetDocumentationFileInUnityProjectIfEmptyDebug()
        {
            throw new NotImplementedException();

            /*
            public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
            public static bool SetDocumentationFileInUnityProjectIfEmpty(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetSetDocumentationFileInUnityProjectIfEmptyRelease()
        {
            throw new NotImplementedException();

            /*
            public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
            public static bool SetDocumentationFileInUnityProjectIfEmpty(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetSetDocumentationFileIfEmptyDebug()
        {
            throw new NotImplementedException();

            /*
            public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
            public static bool SetDocumentationFileIfEmpty(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetSetDocumentationFileIfEmptyRelease()
        {
            throw new NotImplementedException();

            /*
            public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
            public static bool SetDocumentationFileIfEmpty(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetSetDocumentationFileDebug()
        {
            throw new NotImplementedException();

            /*
            public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
            public static bool SetDocumentationFile(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }

        [Test]
        public void TestGetSetDocumentationFileRelease()
        {
            throw new NotImplementedException();

            /*
            public static string GetDocumentationFile(string projectFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
            public static bool SetDocumentationFile(string projectFileName, string documentationFileName, KindOfConfiguration kindOfConfiguration = KindOfConfiguration.Debug)
*/
        }
    }
}

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