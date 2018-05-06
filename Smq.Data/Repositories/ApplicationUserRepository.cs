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
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        bool UpdateUserInformation(string id, string fullName, string address, DateTime birthDay, string email, string phoneNumber);
    }
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public bool UpdateUserInformation(string id,string fullName,string address, DateTime birthDay,string email,string phoneNumber)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@FullName",fullName),
                new SqlParameter("@Address",address),
                new SqlParameter("@BirthDay",birthDay),
                new SqlParameter("@Email",email),
                new SqlParameter("@PhoneNumber",phoneNumber)};
                DbContext.Database.ExecuteSqlCommand("EXEC UpdateUserInformation @Id,@FullName,@Address,@BirthDay,@Email,@PhoneNumber", parameters);
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
