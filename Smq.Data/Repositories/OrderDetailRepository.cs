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
        bool DeleteOrderDetails(int orderId,int productId);
        bool UpdateOrderDetails(int orderId, int productId, int quantity);
        IEnumerable<OrderDetail> GetAllOrderDetail();
    }
    public class OrderDetailRepository : RepositoryBase<OrderDetail>,IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public bool DeleteOrderDetails(int orderId, int productId)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@OrderID",orderId),
                new SqlParameter("@ProductID",productId) };
                DbContext.Database.ExecuteSqlCommand("EXEC DeleteOrderDetails @OrderID,@ProductID", parameters);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<OrderDetail> GetAllOrderDetail()
        {
            return DbContext.Database.SqlQuery<OrderDetail>("EXEC GetAllOrderDetail").ToList();
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

        public bool UpdateOrderDetails(int orderId, int productId, int quantity)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@OrderID",orderId),
                new SqlParameter("@ProductID",productId),
                new SqlParameter("@Quantity",quantity)};
                DbContext.Database.ExecuteSqlCommand("EXEC UpdateOrderDetails @OrderID,@ProductID,@Quantity", parameters);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
