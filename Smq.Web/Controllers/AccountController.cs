﻿using Smq.Web.Models;
using BotDetect.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Smq.Model.Models;
using Smq.Common;
using Smq.Web.App_Start;
using System.Net;
using AutoMapper;
using Smq.Service;
using System.Globalization;

namespace Smq.Web.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IApplicationUserService _applicationUserService;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IApplicationUserService applicationUserService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _applicationUserService = applicationUserService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AccountController()
        {

        }
        // GET: Account
        public ActionResult Login(string returUrl)
        {
            ViewBag.ReturnUrl = returUrl;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (model.UserName == null && model.Password == null)
            {
                ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View(model);
            }
            else
            {
                if(model.UserName != null && model.Password == null)
                {
                    ViewBag.Message = "Phải nhập mật khẩu!";
                }
                else if(model.UserName == null && model.Password != null)
                {
                    ViewBag.Message = "Phải nhập tài khoản!";
                }
                else
                { 
                    ApplicationUser user = _userManager.Find(model.UserName, model.Password);
                    if (user != null)
                    {
                        IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                        authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationProperties props = new AuthenticationProperties();
                        props.IsPersistent = model.RememberMe;
                        authenticationManager.SignIn(props, identity);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    }
                }
                return View(model);
            }         
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            ControllerContext.HttpContext.Session.RemoveAll();
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Incorrect code")]
        [ValidateInput(false)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email already exists");
                    return View(model);
                }
                var userByUserName = await _userManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("email", "Account already exists");
                    return View(model);
                }
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address

                };
                await _userManager.CreateAsync(user, model.Password);
                var adminUser = await _userManager.FindByEmailAsync(model.Email);
                //if (adminUser != null)
                //    await _userManager.AddToRolesAsync(adminUser.Id, new string[] { "User" });

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
                content = content.Replace("{{UserName}}", adminUser.FullName);
                content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "dang-nhap.html");

                MailHelper.SendMail(adminUser.Email, "Đăng ký thành công !", content);

                ViewData["SuccessMsg"] = "Đăng ký thành công!";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult YourInformation()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {          
                var userObj = _userManager.Users.Where(n => n.Id == userId);
                var userModel = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(userObj);
                return View(userModel);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult YourInformation(string id, string fullName, string address, string birthDay, string email, string phoneNumber)
        {
            var userId = User.Identity.GetUserId();

            if (userId != null && birthDay != null)
            {
                var birthDayFormat = DateTime.ParseExact(birthDay, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _applicationUserService.UpdateUserInformation(id,fullName,address, birthDayFormat, email,phoneNumber);
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
        [HttpPost] 
        public JsonResult ChangePassword(string currentPassword,string newPassword)
        {
            var userId = User.Identity.GetUserId();      
            if (userId != null && currentPassword != null && newPassword != null)
            {
                var checkPassword = _userManager.Find(User.Identity.GetUserName(), currentPassword);
                if (checkPassword != null)
                {
                    _userManager.ChangePassword(userId, currentPassword, newPassword);
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }           
            }
            return Json(new
            {
                status = false
            });
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}