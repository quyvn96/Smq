using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smq.Data.Infrastructure;

namespace Smq.Data.Repositories
{
    public interface ISystemConfigRepository : IRepository<SystemConfig>
    {

    }
    public class SystemConfigRepository : RepositoryBase<SystemConfig>,ISystemConfigRepository
    {
        public SystemConfigRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }
    }
}
