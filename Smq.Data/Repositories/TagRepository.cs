using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smq.Data.Infrastructure;

namespace Smq.Data.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {

    }
    public class TagRepository : RepositoryBase<Tag>,ITagRepository
    {
        public TagRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }
    }
}
