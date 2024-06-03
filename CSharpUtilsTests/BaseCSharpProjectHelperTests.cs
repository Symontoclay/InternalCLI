using CommonUtils;
using CSharpUtils;

namespace CSharpUtilsTests
{
    public abstract class BaseCSharpProjectHelperTests
    {
        protected string CreateTestCsProjectFile(KindOfTargetCSharpFramework kindOfTargetCSharpFramework, TempDirectory tempDirectory)
        {
            var csProjectSourceFileName = GetTestCsProjectFileSource(kindOfTargetCSharpFramework);

            var oldFullFileName = Path.Combine(Directory.GetCurrentDirectory(), "Projects", csProjectSourceFileName);

            var newFullFileName = Path.Combine(tempDirectory.FullName, csProjectSourceFileName.Replace(".xml", ".csproj"));

            File.Copy(oldFullFileName, newFullFileName, true);

            return newFullFileName;
        }

        private string GetTestCsProjectFileSource(KindOfTargetCSharpFramework kindOfTargetCSharpFramework)
        {
            switch (kindOfTargetCSharpFramework)
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
