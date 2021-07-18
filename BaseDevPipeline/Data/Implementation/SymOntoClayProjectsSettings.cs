using CommonUtils.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline.Data.Implementation
{
    public class SymOntoClayProjectsSettings: ISymOntoClayProjectsSettings
    {
        /// <inheritdoc/>
        public string BasePath { get; set; }

        public string SecretFilePath { get; set; }

        public List<UtityExeInstance> UtityExeInstances { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IUtityExeInstance> ISymOntoClayProjectsSettings.UtityExeInstances => UtityExeInstances;

        public List<SolutionSettings> Solutions { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<ISolutionSettings> ISymOntoClayProjectsSettings.Solutions => Solutions;

        public List<ProjectSettings> Projects { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IProjectSettings> ISymOntoClayProjectsSettings.Projects => Projects;

        public List<ArtifactSettings> Artifacts { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<IArtifactSettings> ISymOntoClayProjectsSettings.Artifacts => Artifacts;

        public List<LicenseSettings> Licenses { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<ILicenseSettings> ISymOntoClayProjectsSettings.Licenses => Licenses;

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
        public IArtifactSettings GetArtifact(KindOfArtifact kind)
        {
            return GetArtifacts(kind).SingleOrDefault();
        }

        /// <inheritdoc/>
        public IReadOnlyList<IArtifactSettings> GetArtifacts(KindOfArtifact kind)
        {
            if (_artifactsDict.ContainsKey(kind))
            {
                return _artifactsDict[kind];
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
            _projectsDict = Projects.GroupBy(p => p.Kind).ToDictionary(p => p.Key, p => p.Cast<IProjectSettings>().ToList());
            _artifactsDict = Artifacts.GroupBy(p => p.Kind).ToDictionary(p => p.Key, p => p.Cast<IArtifactSettings>().ToList());

            _licesnsesDict = Licenses.Cast<ILicenseSettings>().ToDictionary(p => p.Name, p => p);
        }

        private Dictionary<KindOfProject, List<ISolutionSettings>> _solutionsDict;
        private Dictionary<KindOfProject, List<IProjectSettings>> _projectsDict;
        private Dictionary<KindOfArtifact, List<IArtifactSettings>> _artifactsDict;
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
            sb.AppendLine($"{spaces}{nameof(SecretFilePath)} = {SecretFilePath}");
            sb.PrintObjListProp(n, nameof(UtityExeInstances), UtityExeInstances);
            sb.PrintObjListProp(n, nameof(Solutions), Solutions);
            sb.PrintObjListProp(n, nameof(Projects), Projects);
            sb.PrintObjListProp(n, nameof(Artifacts), Artifacts);
            sb.PrintObjListProp(n, nameof(Licenses), Licenses);

            return sb.ToString();
        }
    }
}
