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
         IEnumerable<Tag> GetTagByProductId(int productId);
         IEnumerable<Tag> GetAllTagPost();
    }
    public class TagRepository : RepositoryBase<Tag>,ITagRepository
    {
        public TagRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public IEnumerable<Tag> GetAllTagPost()
        {
            var query = from p in DbContext.Tags
                        join pt in DbContext.PostTags
                        on p.ID equals pt.TagID
                        select p;
            return query;
        }

        public IEnumerable<Tag> GetTagByProductId(int productId)
        {
            var query = from p in DbContext.Tags
                        join pt in DbContext.ProductTags
                        on p.ID equals pt.TagID
                        where pt.ProductID == productId
                        select p;
            return query;
        }
    }
}
