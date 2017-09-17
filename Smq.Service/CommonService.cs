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
        IEnumerable<Slide> GetSlides();
    }
    public class CommonService: ICommonService
    {
        IFooterRepository _footerReponsitory;
        IUnitOfWork _unitOfWork;
        ISlideRepository _slideRepository;
        public CommonService(IFooterRepository footerReponsitory, IUnitOfWork unitOfWork, ISlideRepository slideRepository)
        {
            this._footerReponsitory = footerReponsitory;
            this._unitOfWork = unitOfWork;
            this._slideRepository = slideRepository;
        }
        public Footer GetFooter()
        {
            return _footerReponsitory.GetSingleByCondition(n => n.ID == CommonConstants.DefaultFooterId); 
        }


        public IEnumerable<Slide> GetSlides()
        {
           return  _slideRepository.GetMulti(x=>x.Status);
        }
    }
}
