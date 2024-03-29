﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Smq.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                    name: "Contact",
                    url: "lien-he.html",
                    defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "About",
                    url: "gioi-thieu.html",
                    defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Confirm Order",
                    url: "xac-nhan-don-hang.html",
                    defaults: new { controller = "ShoppingCart", action = "ConfirmOrder", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Cancel Order",
                    url: "huy-don-hang.html",
                    defaults: new { controller = "ShoppingCart", action = "CancelOrder", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "AllProduct",
                    url: "san-pham.html",
                    defaults: new { controller = "Product", action = "AllProduct"},
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                   name: "AllProductLastest",
                   url: "san-pham-moi-nhat.html",
                   defaults: new { controller = "Product", action = "AllProductLastest"},
                   namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                   name: "AllProductHot",
                   url: "san-pham-noi-bat.html",
                   defaults: new { controller = "Product", action = "AllProductHot"},
                   namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                  name: "ProductByPrice",
                  url: "san-pham-theo-gia-{id}.html",
                  defaults: new { controller = "Product", action = "ProductByPrice" },
                  namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "AllPost",
                    url: "tat-ca-bai-viet.html",
                    defaults: new { controller = "Post", action = "AllPost" },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Search",
                    url: "tim-kiem.html",
                    defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                     name: "Login",
                     url: "dang-nhap.html",
                     defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                     namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Register",
                    url: "dang-ky.html",
                    defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Cart",
                    url: "gio-hang.html",
                    defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "CheckOut",
                    url: "thanh-toan.html",
                    defaults: new { controller = "ShoppingCart", action = "CheckOut", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Page",
                    url: "trang/{alias}.html",
                    defaults: new { controller = "Page", action = "Index", alias = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Product Category",
                    url: "{alias}.pc-{id}.html",
                    defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Product",
                    url: "{alias}.p-{id}.html",
                    defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "TagList",
                    url: "tag/{tagId}.html",
                    defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "PostTagList",
                    url: "tagbaiviet/{tagId}.html",
                    defaults: new { controller = "Post", action = "PostByTag", tagId = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Post",
                    url: "baiviet/{id}.html",
                    defaults: new { controller = "Post", action = "Index", tagId = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "YourInformation",
                    url: "taikhoan.html",
                    defaults: new { controller = "Account", action = "YourInformation"},
                    namespaces: new string[] { "Smq.Web.Controllers" });

            routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    namespaces: new string[] { "Smq.Web.Controllers" });
        }
    }
}