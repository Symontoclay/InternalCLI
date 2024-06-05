﻿using CommonUtils;
using CSharpUtils;

namespace CSharpUtilsTests
{
    public class CSharpProjectHelperNetFrameworkTests: BaseCSharpProjectHelperTests
    {
        private KindOfTargetCSharpFramework _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetFramework;

        [Test]
        public void TestGetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("v4.7.1"));

            CSharpProjectHelper.SetTargetFramework(projectFileName, "v4.7.2");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("v4.7.2"));
        }

        [Test]
        public void TestGetSetTargetFrameworkVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.NetFramework, new Version(4, 7, 1))));

            CSharpProjectHelper.SetTargetFramework(projectFileName, (KindOfTargetCSharpFramework.NetFramework, new Version(4, 7, 2)));

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.NetFramework, new Version(4, 7, 2))));
        }

        [Test]
        public void TestGetGeneratePackageOnBuild()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var result = CSharpProjectHelper.GetGeneratePackageOnBuild(projectFileName);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void TestGetAssemblyName()
        {
            throw new NotImplementedException();

            /*
            public static string GetAssemblyName(string projectFileName)
*/
        }

        [Test]
        public void TestGetPackageId()
        {
            throw new NotImplementedException();

            /*
            public static string GetPackageId(string projectFileName)
*/
        }

        [Test]
        public void TestGetInstalledPackages()
        {
            throw new NotImplementedException();

            /*
            public static List<(string PackageId, Version Version)> GetInstalledPackages(string projectFileName)
*/
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
        public void TestSetCopyright()
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