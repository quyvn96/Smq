using AutoMapper;
using Smq.Common;
using Smq.Model.Models;
using Smq.Service;
using Smq.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Smq.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ICommonService _commonService;
        IProductService _productService;
        IPostCategoryService _postCategoryService;
        IMenuService _menuService;
        public HomeController(IProductCategoryService productCategoryService,ICommonService commonService,IProductService productService, IPostCategoryService postCategoryService, IMenuService menuService)
        {
            this._productCategoryService = productCategoryService;
            this._commonService = commonService;
            this._productService = productService;
            this._postCategoryService = postCategoryService;
            this._menuService = menuService;
        }
        [OutputCache(Duration = 60,Location=OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlides();
            var slideView = Mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slideModel);
            var homeView = new HomeViewModel();
            homeView.Slides = slideView;

            var lastestProductModel = _productService.GetLastest(3);
            var topSaleProductModel = _productService.GetHotProduct(3);
            var lastestProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            var topSaleProductViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);
            homeView.LastestProducts = lastestProductViewModel;
            homeView.TopSaleProducts = topSaleProductViewModel;
            try
            {
                homeView.Title = _commonService.GetSystemConfig(CommonConstants.HomeTitle).ValueString;
                homeView.MetaKeyword = _commonService.GetSystemConfig(CommonConstants.HomeMetaKeyword).ValueString;
                homeView.MetaDescription = _commonService.GetSystemConfig(CommonConstants.HomeDescription).ValueString;
            }
            catch
            {

            }
            return View(homeView);
        }
        [ChildActionOnly]
        [OutputCache(Duration=3600)]
        public ActionResult Footer()
        {
            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer, FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }
        [ChildActionOnly]
        public ActionResult Header()
        {
            var listPostCategory= _postCategoryService.GetAll();
            var listPostCategoryParent = listPostCategory.Where(n => n.ParentID == null);
            var listPostCategoryChild = listPostCategory.Where(n => n.ParentID != null);
            var listPostCategoryParentVM = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(listPostCategoryParent);
            var listPostCategoryChildVM = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(listPostCategoryChild);
            ViewBag.PostCategoryParent = listPostCategoryParentVM;
            ViewBag.PostCategoryChild = listPostCategoryChildVM;

            var listMenu = _menuService.GetAll();
            var lisMenuVM = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(listMenu);
            var listParentMenu = listMenu.Where(n => n.GroupID == 1);
            var listSpecialMenu = listMenu.Where(n => n.GroupID == 3);
            var listParentMenuVM = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(listParentMenu);
            var listSpecialMenuVM = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(listSpecialMenu);
            ViewBag.ParentMenu = listParentMenuVM;
            ViewBag.SpecialMenu = listSpecialMenuVM;
            return PartialView();
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            var listProduct = _productService.GetAll();
            var listProductVM = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(listProduct);
            var specialProduct = new List<ProductViewModel>();
            foreach (var item in listProductVM)
            {
                if(item.Description != null && item.Description.ToLower() == "special" && item.HomeFlag == false && item.HotFlag == false)
                {
                    specialProduct.Add(new ProductViewModel
                    {
                        Name = item.Name,
                        Price = item.Price,
                        PromotionPrice = item.PromotionPrice,
                        Image = item.Image
                    });
                }
            }
            if(specialProduct.Count() > 0)
            {
                ViewBag.SpecialProduct = specialProduct;
            }
            else
            {
                new List<ProductViewModel>();
            }
            return PartialView(listProductCategoryViewModel);
        }
    }
}