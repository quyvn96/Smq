using AutoMapper;
using Smq.Common;
using Smq.Model.Models;
using Smq.Service;
using Smq.Web.Infrastructure.Core;
using Smq.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Smq.Web.Api
{
    [RoutePrefix("api/feedback")]
    [Authorize]
    public class FeedbackController : ApiControllerBase
    {
        private IFeedbackService _feebackService;
        public FeedbackController(IErrorService errorService, IFeedbackService feebackService) :base(errorService)
        {
            this._feebackService = feebackService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _feebackService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(query);

                var paginationSet = new PaginationSet<FeedbackViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }
        [Route("updatestatus")]
        [HttpGet]
        public HttpResponseMessage DeleteOrder(HttpRequestMessage request, int id,bool status)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var orderObject = _feebackService.UpdateFeedbackStatus(id, status);
                    _feebackService.Save();

                    var responseData = orderObject;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var orderObject = _feebackService.DeleteFeedback(id);
                    _feebackService.Save();

                    var responseData = orderObject;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [HttpGet]
        [Route("exportfeedback")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, string filter = null)
        {
            string fileName = string.Concat("Feedback_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _feebackService.GetAll(filter).ToList();
                await ReportHelper.GenerateXls(data, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
