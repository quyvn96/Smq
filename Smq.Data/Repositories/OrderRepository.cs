using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smq.Data.Infrastructure;

using Smq.Common.ViewModels;
using System.Data.SqlClient;
namespace Smq.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<RevenueStatisticViewModel> GetGetRevenueStatistic(string fromDate, string toDate);
        IEnumerable<Order> GetAllOrder();
        bool DeleteOrder(int Id);
        bool UpdateOrderStatus(int id, bool status);
    }
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public bool DeleteOrder(int id)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@ID",id) };
                DbContext.Database.ExecuteSqlCommand("EXEC DeleteOrder @ID", parameters);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<Order> GetAllOrder()
        {
            return DbContext.Database.SqlQuery<Order>("EXEC GetAllOrder").ToList();
        }

        public IEnumerable<RevenueStatisticViewModel> GetGetRevenueStatistic(string fromDate = null, string toDate = null)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("GetRevenueStatistic @fromDate,@toDate", parameters);
        }

        public bool UpdateOrderStatus(int id, bool status)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@Status",status)};
                DbContext.Database.ExecuteSqlCommand("EXEC UpdateOrderStatus @Id,@Status", parameters);
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }
    }
}
