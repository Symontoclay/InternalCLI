﻿using SymOntoClay.Common;

namespace BaseDevPipeline.Data
{
    public interface IProjectSettings : IObjectToString
    {
        ISolutionSettings Solution { get; }
        KindOfProject Kind { get; }
        string Path { get; }
        string CsProjPath { get; }
        string LicenseName { get; }
        ILicenseSettings License { get; }
    }
}
