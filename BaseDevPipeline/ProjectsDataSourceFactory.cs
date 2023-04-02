using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public static class ProjectsDataSourceFactory
    {
        public static ProjectsDataSourceMode Mode { get; set; } = ProjectsDataSourceMode.Prod;
    }
}
