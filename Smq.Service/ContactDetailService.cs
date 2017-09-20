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
    public interface IContactDetailService
    {
        ContactDetail GetDefaultContact();
    }
    public class ContactDetailService : IContactDetailService
    {
        IContactDetailRepository _contactDetailRepository;
        IUnitOfWork _unitOfWork;
        public ContactDetailService(IContactDetailRepository contactDetailRepository, IUnitOfWork unitOfWork)
        {
            this._contactDetailRepository = contactDetailRepository;
            this._unitOfWork = unitOfWork;
        }
        public ContactDetail GetDefaultContact()
        {
            return _contactDetailRepository.GetSingleByCondition(n => n.Status);
        }
    }
}
