using Smq.Data.Infrastructure;
using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Data.Repositories
{
    public interface IFeedbackRepository:IRepository<Feedback>
    {
        bool UpdateFeedbackStatus(int id, bool status);
    }
    public class FeedbackRepository : RepositoryBase<Feedback>,IFeedbackRepository
    {
        public FeedbackRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {

        }

        public bool UpdateFeedbackStatus(int id,bool status)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@Status",status)};
                DbContext.Database.ExecuteSqlCommand("EXEC UpdateFeedbackStatus @Id,@Status", parameters);
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
