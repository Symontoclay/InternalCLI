using CommonUtils.DebugHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public class SymOntoClayProjectsSettings: ISymOntoClayProjectsSettings
    {
        public string BasePath { get; set; }

        public string SecretFilePath { get; set; }

        public string CommonReadmeSource { get; set; }
        public string CommonBadgesSource { get; set; }

        public string CodeOfConductSource { get; set; }
        public string ContributingSource { get; set; }

        public string Copyright { get; set; }

        public string InternalCLIDist { get; set; }
        public string SocExePath { get; set; }

        /// <inheritdoc/>
        public string GetSecret(string key)
        {
            return GetSecrets()[key];
        }

        /// <inheritdoc/>
        public Dictionary<string, string> GetSecrets()
        {
            return SecretFile.ReadSecrets(SecretFilePath);
        }

        public List<UtityExeInstance> UtityExeInstances { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IUtityExeInstance> ISymOntoClayProjectsSettings.UtityExeInstances => UtityExeInstances;

        public List<SolutionSettings> Solutions { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<ISolutionSettings> ISymOntoClayProjectsSettings.Solutions => Solutions;

        public List<ProjectSettings> Projects { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IProjectSettings> ISymOntoClayProjectsSettings.Projects => Projects;

        public List<ArtifactSettings> DevArtifacts { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IArtifactSettings> ISymOntoClayProjectsSettings.DevArtifacts => DevArtifacts;

        public List<LicenseSettings> Licenses { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<ILicenseSettings> ISymOntoClayProjectsSettings.Licenses => Licenses;

        public List<KindOfArtifact> ArtifactsForDeployment { get; set; }

        public string RepositoryReadmeSource { get; set; }
        public string RepositoryBadgesSource { get; set; }

        /// <inheritdoc/>
        public ISolutionSettings GetSolution(KindOfProject kind)
        {
            return GetSolutions(kind).SingleOrDefault();
        }

        /// <inheritdoc/>
        public IReadOnlyList<ISolutionSettings> GetSolutions(KindOfProject kind)
        {
            if(_solutionsDict.ContainsKey(kind))
            {
                return _solutionsDict[kind];
            }

            return _emptySolutions;
        }

        /// <inheritdoc/>
        public IReadOnlyList<ISolutionSettings> GetSolutionsWithMaintainedReleases()
        {
            return _solutionsWithMaintainedReleases;
        }

        /// <inheritdoc/>
        public IReadOnlyList<ISolutionSettings> GetSolutionsWithMaintainedVersionsInCSharpProjects()
        {
            return _solutionsWithMaintainedVersionsInCSharpProjects;
        }

        /// <inheritdoc/>
        public IReadOnlyList<ISolutionSettings> GetUnityExampleSolutions()
        {
            return _unityExampleSolutions;
        }

        /// <inheritdoc/>
        public IProjectSettings GetProject(KindOfProject kind)
        {
            return GetProjects(kind).SingleOrDefault();
        }

        /// <inheritdoc/>
        public IReadOnlyList<IProjectSettings> GetProjects(KindOfProject kind)
        {
            if (_projectsDict.ContainsKey(kind))
            {
                return _projectsDict[kind];
            }

            return _emptyProjects;
        }

        /// <inheritdoc/>
        public IArtifactSettings GetDevArtifact(KindOfArtifact kind)
        {
            return GetDevArtifacts(kind).SingleOrDefault();
        }

        /// <inheritdoc/>
        public IReadOnlyList<IArtifactSettings> GetDevArtifacts(KindOfArtifact kind)
        {
            if (_devArtifactsDict.ContainsKey(kind))
            {
                return _devArtifactsDict[kind];
            }

            return _emptyArtifacts;
        }

        /// <inheritdoc/>
        public ILicenseSettings GetLicense(string name)
        {
            if (_licesnsesDict.ContainsKey(name))
            {
                return _licesnsesDict[name];
            }

            return null;
        }

        public void Prepare()
        {
            _solutionsDict = Solutions.GroupBy(p => p.Kind).ToDictionary(p => p.Key, p => p.Cast<ISolutionSettings>().ToList());
            _solutionsWithMaintainedReleases = Solutions.Where(p => p.Kind == KindOfProject.CoreSolution || p.Kind == KindOfProject.ProjectSite || p.Kind == KindOfProject.Unity).Cast<ISolutionSettings>().ToList();
            _solutionsWithMaintainedVersionsInCSharpProjects = Solutions.Where(p => p.Kind == KindOfProject.CoreSolution || p.Kind == KindOfProject.Unity).Cast<ISolutionSettings>().ToList();
            _unityExampleSolutions = Solutions.Where(p => p.Kind == KindOfProject.UnityExample).Cast<ISolutionSettings>().ToList();
            _projectsDict = Projects.GroupBy(p => p.Kind).ToDictionary(p => p.Key, p => p.Cast<IProjectSettings>().ToList());
            _devArtifactsDict = DevArtifacts.GroupBy(p => p.Kind).ToDictionary(p => p.Key, p => p.Cast<IArtifactSettings>().ToList());

            _licesnsesDict = Licenses.Cast<ILicenseSettings>().ToDictionary(p => p.Name, p => p);

            var solutionForCommonReadme = Solutions.SingleOrDefault(p => p.IsCommonReadmeSource);

            if(solutionForCommonReadme != null)
            {
                CommonReadmeSource = solutionForCommonReadme.CommonReadmeSource;
                CommonBadgesSource = solutionForCommonReadme.CommonBadgesSource;
                CodeOfConductSource = solutionForCommonReadme.CodeOfConductSource;
                ContributingSource = solutionForCommonReadme.ContributingSource;
            }
        }

        private Dictionary<KindOfProject, List<ISolutionSettings>> _solutionsDict;
        private List<ISolutionSettings> _solutionsWithMaintainedReleases;
        private List<ISolutionSettings> _solutionsWithMaintainedVersionsInCSharpProjects;
        private List<ISolutionSettings> _unityExampleSolutions;
        private Dictionary<KindOfProject, List<IProjectSettings>> _projectsDict;
        private Dictionary<KindOfArtifact, List<IArtifactSettings>> _devArtifactsDict;
        private Dictionary<string, ILicenseSettings> _licesnsesDict;

        private IReadOnlyList<ISolutionSettings> _emptySolutions = new List<ISolutionSettings>();
        private IReadOnlyList<IProjectSettings> _emptyProjects = new List<IProjectSettings>();
        private IReadOnlyList<IArtifactSettings> _emptyArtifacts = new List<IArtifactSettings>();

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(0u);
        }

        /// <inheritdoc/>
        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        /// <inheritdoc/>
        string IObjectToString.PropertiesToString(uint n)
        {
            var spaces = DisplayHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(BasePath)} = {BasePath}");
            sb.AppendLine($"{spaces}{nameof(CommonReadmeSource)} = {CommonReadmeSource}");
            sb.AppendLine($"{spaces}{nameof(CommonBadgesSource)} = {CommonBadgesSource}");
            sb.AppendLine($"{spaces}{nameof(CodeOfConductSource)} = {CodeOfConductSource}");
            sb.AppendLine($"{spaces}{nameof(ContributingSource)} = {ContributingSource}");
            sb.AppendLine($"{spaces}{nameof(SecretFilePath)} = {SecretFilePath}");
            sb.PrintObjListProp(n, nameof(UtityExeInstances), UtityExeInstances);
            sb.PrintObjListProp(n, nameof(Solutions), Solutions);
            sb.PrintObjListProp(n, nameof(Projects), Projects);
            sb.PrintObjListProp(n, nameof(DevArtifacts), DevArtifacts);
            sb.PrintObjListProp(n, nameof(Licenses), Licenses);
            sb.AppendLine($"{spaces}{nameof(Copyright)} = {Copyright}");
            sb.AppendLine($"{spaces}{nameof(InternalCLIDist)} = {InternalCLIDist}");
            sb.AppendLine($"{spaces}{nameof(SocExePath)} = {SocExePath}");

            return sb.ToString();
        }
    }
}
