using CommonUtils;
using CSharpUtils;

namespace CSharpUtilsTests
{
    public class CSharpProjectHelperNetWindowsTests : BaseCSharpProjectHelperTests
    {
        private KindOfTargetCSharpFramework _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetWindows;

        [Test]
        public void TestGetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("net7.0-windows"));

            CSharpProjectHelper.SetTargetFramework(projectFileName, "net8.0-windows");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("net8.0-windows"));
        }

        [Test]
        public void TestGetSetTargetFrameworkVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.NetWindows, new Version(7, 0))));

            CSharpProjectHelper.SetTargetFramework(projectFileName, (KindOfTargetCSharpFramework.NetWindows, new Version(8, 0)));

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.NetWindows, new Version(8, 0))));
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

            Assert.That(packagesList.Count, Is.EqualTo(2));

            var item1 = packagesList[0];

            Assert.That(item1.PackageId, Is.EqualTo("NLog"));
            Assert.That(item1.Version, Is.EqualTo(new Version("5.1.4")));

            var item2 = packagesList[1];

            Assert.That(item2.PackageId, Is.EqualTo("OxyPlot.Wpf"));
            Assert.That(item2.Version, Is.EqualTo(new Version("2.1.0")));
        }

        [Test]
        public void TestGetUpdateInstalledPackageVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var packageId = "NLog";

            var packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

            Assert.That(packageVersion, Is.EqualTo("5.1.4"));

            var result = CSharpProjectHelper.UpdateInstalledPackageVersion(projectFileName, packageId, "5.1.5");

            Assert.That(result, Is.EqualTo(true));

            packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

            Assert.That(packageVersion, Is.EqualTo("5.1.5"));
        }

        [Test]
        public void TestGetSetVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var version = CSharpProjectHelper.GetVersion(projectFileName);

            Assert.That(version, Is.EqualTo("0.5.4"));

            var result = CSharpProjectHelper.SetVersion(projectFileName, "0.5.5");

            Assert.That(result, Is.EqualTo(true));

            version = CSharpProjectHelper.GetVersion(projectFileName);

            Assert.That(version, Is.EqualTo("0.5.5"));
        }

        [Test]
        public void TestGetSetCopyright()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var copyright = CSharpProjectHelper.GetCopyright(projectFileName);

            Assert.That(copyright, Is.EqualTo("Copyright (c) 2020 - 2024 Sergiy Tolkachov aka metatypeman"));

            var result = CSharpProjectHelper.SetCopyright(projectFileName, "Copyright (c) Tst");

            Assert.That(result, Is.EqualTo(true));

            copyright = CSharpProjectHelper.GetCopyright(projectFileName);

            Assert.That(copyright, Is.EqualTo("Copyright (c) Tst"));
        }

        [Test]
        public void TestGetOutputPathDebug()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var outputPathDebug = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Debug);

            Assert.That(outputPathDebug, Is.EqualTo(""));
        }

        [Test]
        public void TestGetOutputPathRelease()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var outputPathRelease = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Release);

            Assert.That(outputPathRelease, Is.EqualTo(""));
        }

        [Test]
        public void TestGetSetDocumentationFileInUnityProjectIfEmptyDebug()
        {
            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo(""));

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

            Assert.That(result, Is.EqualTo(true));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("Assembly-CSharp.xml"));
        }

        [Test]
        public void TestGetSetDocumentationFileInUnityProjectIfEmptyRelease()
        {
            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo(""));

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

            Assert.That(result, Is.EqualTo(true));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("Assembly-CSharp.xml"));
        }

        [Test]
        public void TestGetSetDocumentationFileIfEmptyDebug()
        {
            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo(""));

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

            Assert.That(result, Is.EqualTo(true));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SomeDoc.xml"));
        }

        [Test]
        public void TestGetSetDocumentationFileIfEmptyRelease()
        {
            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo(""));

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

            Assert.That(result, Is.EqualTo(true));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SomeDoc.xml"));
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