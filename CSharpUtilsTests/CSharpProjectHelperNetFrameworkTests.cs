using CommonUtils;
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
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var assemblyName = CSharpProjectHelper.GetAssemblyName(projectFileName);

            Assert.That(assemblyName, Is.EqualTo("Assembly-CSharp"));
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

            Assert.That(packagesList.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGetUpdateInstalledPackageVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var packageId = "NLog";

            var packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

            Assert.That(packageVersion, Is.EqualTo(""));

            var result = CSharpProjectHelper.UpdateInstalledPackageVersion(projectFileName, packageId, "5.1.5");

            Assert.That(result, Is.EqualTo(false));

            packageVersion = CSharpProjectHelper.GetInstalledPackageVersion(projectFileName, packageId);

            Assert.That(packageVersion, Is.EqualTo(""));
        }

        [Test]
        public void TestGetSetVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var version = CSharpProjectHelper.GetVersion(projectFileName);

            Assert.That(version, Is.EqualTo(""));

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

            Assert.That(copyright, Is.EqualTo(""));

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

            Assert.That(outputPathDebug, Is.EqualTo(@"Temp\bin\Debug\"));
        }

        [Test]
        public void TestGetOutputPathRelease()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var outputPathRelease = CSharpProjectHelper.GetOutputPath(projectFileName, KindOfConfiguration.Release);

            Assert.That(outputPathRelease, Is.EqualTo(@"Temp\bin\Release\"));
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

            Assert.That(documentationFile, Is.EqualTo(@"Temp\bin\Debug\Assembly-CSharp.xml"));
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

            Assert.That(documentationFile, Is.EqualTo(@"Temp\bin\Release\Assembly-CSharp.xml"));
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

            Assert.That(documentationFile, Is.EqualTo(@"Temp\bin\Debug\SomeDoc.xml"));
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

            Assert.That(documentationFile, Is.EqualTo(@"Temp\bin\Release\SomeDoc.xml"));
        }

        [Test]
        public void TestGetSetDocumentationFileDebug()
        {
            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo(""));

            var result = CSharpProjectHelper.SetDocumentationFile(projectFileName, "SomeDoc.xml", kindOfConfiguration);

            Assert.That(result, Is.EqualTo(true));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SomeDoc.xml"));
        }

        [Test]
        public void TestGetSetDocumentationFileRelease()
        {
            var kindOfConfiguration = KindOfConfiguration.Release;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo(""));

            var result = CSharpProjectHelper.SetDocumentationFile(projectFileName, "SomeDoc.xml", kindOfConfiguration);

            Assert.That(result, Is.EqualTo(true));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SomeDoc.xml"));
        }
    }
}
