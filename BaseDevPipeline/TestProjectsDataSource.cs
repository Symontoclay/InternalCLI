using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDevPipeline
{
    public class TestProjectsDataSource: ProjectsDataSource
    {
        private static TestProjectsDataSource _instance;
        private static object _instanceLock = new object();

        public static TestProjectsDataSource Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new TestProjectsDataSource();
                    }
                }

                return _instance;
            }
        }

        public TestProjectsDataSource()
            : base("TestProjectsDataSource.json")
        {
        }
    }
}
