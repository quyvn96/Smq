using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smq.Data.Infrastructure;
using System.Data.SqlClient;

namespace Smq.Data.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        IEnumerable<OrderDetail> GetOrderDetailById(int id);
    }
    public class OrderDetailRepository : RepositoryBase<OrderDetail>,IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public IEnumerable<OrderDetail> GetOrderDetailById(int id)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@ID",id)
            };
                return DbContext.Database.SqlQuery<OrderDetail>("GetOrderDetailById @ID", parameters);
            }
            catch(Exception ex)
            {
                return new List<OrderDetail>();
            }
        }
    }
}
