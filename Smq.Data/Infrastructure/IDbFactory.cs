using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        SmqSolutionDbContext Init();
    }
}
