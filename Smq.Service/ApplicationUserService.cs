using Smq.Data.Infrastructure;
using Smq.Data.Repositories;
using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Service
{
    public interface IApplicationUserService
    {
        bool UpdateUserInformation(string id, string fullName, string address, DateTime birthDay, string email, string phoneNumber);
    }

    public class ApplicationUserService:IApplicationUserService
    {
        IUnitOfWork _unitOfWork;
        IApplicationUserRepository _applicationUserRepository;
        public ApplicationUserService(IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository)
        {
            _unitOfWork = unitOfWork;
            _applicationUserRepository = applicationUserRepository;
        }

        public bool UpdateUserInformation(string id, string fullName, string address, DateTime birthDay, string email, string phoneNumber)
        {
            try
            {
                return _applicationUserRepository.UpdateUserInformation(id,fullName,address,birthDay,email,phoneNumber);
            }catch(Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
        }
    }
}
