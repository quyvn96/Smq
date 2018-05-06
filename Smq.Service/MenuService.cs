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
    public interface IMenuService
    {
        IEnumerable<Menu> GetAll();
    }
    public class MenuService:IMenuService
    {
        IMenuRepository _menuRepository;
        IUnitOfWork _unitOfWork;
        public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            this._menuRepository = menuRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Menu> GetAll()
        {
            try
            {
                var listMenu = _menuRepository.GetAll().Where(n=>n.Status);
                return listMenu;
            }catch(Exception e)
            {
                Console.Write(e.Message);
                return new List<Menu>();
            }
        }
    }
}
