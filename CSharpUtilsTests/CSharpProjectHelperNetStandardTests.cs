using CommonUtils;
using CSharpUtils;
using NUnit.Framework.Interfaces;
using System;

namespace CSharpUtilsTests
{
    public class CSharpProjectHelperNetStandardTests : BaseCSharpProjectHelperTests
    {
        private KindOfTargetCSharpFramework _kindOfTargetCSharpFramework = KindOfTargetCSharpFramework.NetStandard;

        [Test]
        public void TestGetSetTargetFramework()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("netstandard2.0"));

            CSharpProjectHelper.SetTargetFramework(projectFileName, "netstandard2.1");

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFramework(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo("netstandard2.1"));
        }

        [Test]
        public void TestGetSetTargetFrameworkVersion()
        {
            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.NetStandard, new Version(2, 0))));

            CSharpProjectHelper.SetTargetFramework(projectFileName, (KindOfTargetCSharpFramework.NetStandard, new Version(2, 1)));

            targetFrameworkVersion = CSharpProjectHelper.GetTargetFrameworkVersion(projectFileName);

            Assert.That(targetFrameworkVersion, Is.EqualTo((KindOfTargetCSharpFramework.NetStandard, new Version(2, 1))));
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

            Assert.That(assemblyName, Is.EqualTo("SymOntoClay.Core"));
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

            Assert.That(packagesList.Count, Is.EqualTo(3));

            var item1 = packagesList[0];

            Assert.That(item1.PackageId, Is.EqualTo("Newtonsoft.Json"));
            Assert.That(item1.Version, Is.EqualTo(new Version("13.0.3")));

            var item2 = packagesList[1];

            Assert.That(item2.PackageId, Is.EqualTo("NLog"));
            Assert.That(item2.Version, Is.EqualTo(new Version("5.1.4")));

            var item3 = packagesList[2];

            Assert.That(item3.PackageId, Is.EqualTo("System.Numerics.Vectors"));
            Assert.That(item3.Version, Is.EqualTo(new Version("4.5.0")));
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

            Assert.That(documentationFile, Is.EqualTo("SymOntoClay.Core.xml"));

            var result = CSharpProjectHelper.SetDocumentationFileInUnityProjectIfEmpty(projectFileName, kindOfConfiguration);

            Assert.That(result, Is.EqualTo(false));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SymOntoClay.Core.xml"));
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

            Assert.That(documentationFile, Is.EqualTo("SymOntoClay.Core.xml"));

            var result = CSharpProjectHelper.SetDocumentationFileIfEmpty(projectFileName, "SomeDoc.xml", kindOfConfiguration);

            Assert.That(result, Is.EqualTo(false));

            documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SymOntoClay.Core.xml"));
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
            var kindOfConfiguration = KindOfConfiguration.Debug;

            using var tempDir = new TempDirectory();

            var projectFileName = CreateTestCsProjectFile(_kindOfTargetCSharpFramework, tempDir);

            var documentationFile = CSharpProjectHelper.GetDocumentationFile(projectFileName, kindOfConfiguration);

            Assert.That(documentationFile, Is.EqualTo("SymOntoClay.Core.xml"));

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
