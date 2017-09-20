﻿using Smq.Data.Infrastructure;
using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Data.Repositories
{
    public interface IContactDetailRepository : IRepository<ContactDetail>
    {

    }
    public class ContactDetailRepository : RepositoryBase<ContactDetail>,IContactDetailRepository
    {
        public ContactDetailRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
    }
}
