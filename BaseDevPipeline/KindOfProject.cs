namespace BaseDevPipeline
{
    public enum KindOfProject
    {
        Unknown,
        Organization,
        ProjectSite,
        CoreSolution,
        Unity,
        CLI,
        InternalCLI,
        CoreLib,
        CoreAssetLib,
        CorePlugin,
        Library,
        UnitTest,
        IntegrationTest,
        AdditionalApp,
        ReleaseMngrSolution,
        /// <summary>
        /// Describes repository with Unity example demoscene.
        /// </summary>
        UnityExample,
        InternalCLISolution,        
        CommonPackagesSolution
    }
}
