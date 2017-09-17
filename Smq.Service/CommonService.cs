using Smq.Common;
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
    public interface ICommonService
    {
        Footer GetFooter();
    }
    public class CommonService: ICommonService
    {
        IFooterRepository _footerReponsitory;
        IUnitOfWork _unitOfWork;
        public CommonService(IFooterRepository footerReponsitory, IUnitOfWork unitOfWork)
        {
            this._footerReponsitory = footerReponsitory;
            this._unitOfWork = unitOfWork;
        }
        public Footer GetFooter()
        {
            return _footerReponsitory.GetSingleByCondition(n => n.ID == CommonConstants.DefaultFooterId); 
        }
    }
}
