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
        public HomeController(IProductCategoryService productCategoryService,ICommonService commonService,IProductService productService)
        {
            this._productCategoryService = productCategoryService;
            this._commonService = commonService;
            this._productService = productService;
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
            return PartialView();
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Category()
        {
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}