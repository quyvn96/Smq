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
        SystemConfig GetSystemConfig(string code);
    }
    public class CommonService: ICommonService
    {
        IFooterRepository _footerReponsitory;
        IUnitOfWork _unitOfWork;
        ISlideRepository _slideRepository;
        ISystemConfigRepository _systemConfigRepository;
        public CommonService(IFooterRepository footerReponsitory, IUnitOfWork unitOfWork, ISystemConfigRepository systemConfigRepository, ISlideRepository slideRepository)
        {
            this._footerReponsitory = footerReponsitory;
            this._unitOfWork = unitOfWork;
            this._slideRepository = slideRepository;
            this._systemConfigRepository = systemConfigRepository;
        }
        public Footer GetFooter()
        {
            return _footerReponsitory.GetSingleByCondition(n => n.ID == CommonConstants.DefaultFooterId); 
        }


        public IEnumerable<Slide> GetSlides()
        {
           return  _slideRepository.GetMulti(x=>x.Status);
        }


        public SystemConfig GetSystemConfig(string code)
        {
            return _systemConfigRepository.GetSingleByCondition(n => n.Code == code);
        }
    }
}
