using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public class TestProjectsDataSource: ProjectsDataSource
    {
        public TestProjectsDataSource()
            : base("TestProjectsDataSource.json")
        {
        }
    }
}
