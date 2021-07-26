using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public enum GitRepositoryFileStatus
    {
        Unknown,
        Untracked,
        Modified,
        Added,
        Deleted
    }
}
