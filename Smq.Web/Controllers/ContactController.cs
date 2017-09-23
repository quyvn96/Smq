using AutoMapper;
using Smq.Model.Models;
using Smq.Service;
using Smq.Web.Models;
using System.Web.Mvc;
using Smq.Web.Infrastructure.Extensions;
using BotDetect.Web.Mvc;
using Smq.Common;

namespace Smq.Web.Controllers
{
    public class ContactController : Controller
    {
         IContactDetailService _contactDetailService;
         IFeedbackService _feedbackService;

         public ContactController(IContactDetailService contactDetailService, IFeedbackService feedbackService)
        {
            this._contactDetailService = contactDetailService;
            this._feedbackService = feedbackService;
        }

        // GET: Contact
        public ActionResult Index()
        {        
            FeedbackViewModel viewModel = new FeedbackViewModel();
            viewModel.ContactDetail = GetDetail();
            return View(viewModel);
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode","contactCaptcha","Mã xác nhận sai!")]
        public ActionResult SendFeedback(FeedbackViewModel feedbackViewModel)
        {
            if (ModelState.IsValid)
            {
                Feedback newFeedback = new Feedback();
                newFeedback.UpdateFeedback(feedbackViewModel);
                _feedbackService.Create(newFeedback);
                _feedbackService.Save();
                ViewData["SuccessMsg"] = "Gửi phản hồi thành công";

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/contact_template.html"));
                content = content.Replace("{{Name}}", feedbackViewModel.Name);
                content = content.Replace("{{Email}}", feedbackViewModel.Email);
                content = content.Replace("{{Message}}", feedbackViewModel.Message);
                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

                feedbackViewModel.Name = "";
                feedbackViewModel.Message = "";
                feedbackViewModel.Email = "";
            }
            feedbackViewModel.ContactDetail = GetDetail();
         
            return View("Index", feedbackViewModel);
        }
        private ContactDetailViewModel GetDetail()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactViewModel = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);
            return contactViewModel;
        }

    }
}