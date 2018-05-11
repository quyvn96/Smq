using Smq.Common;
using Smq.Service;
using Smq.Web.Infrastructure.Core;
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
    [RoutePrefix("api/statistic")]
    [Authorize]
    public class StatisticController : ApiControllerBase
    {
        IStatisticService _statisticService;
        public StatisticController(IStatisticService statisticService, IErrorService errorService)
            : base(errorService)
        {
            this._statisticService = statisticService;
        }
        [Route("getrevenues")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, ()=>
            {
                var model = _statisticService.GetGetRevenueStatistic(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [HttpGet]
        [Route("exportstatistic")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, string fromDate = null, string toDate = null)
        {
            string fileName = string.Concat("Statistic_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                if (fromDate == null && toDate == null)
                {
                    fromDate = string.Empty;
                    toDate = string.Empty;
                    var data = _statisticService.GetGetRevenueStatistic(fromDate, toDate).ToList();
                    await ReportHelper.GenerateXls(data, fullPath);
                    return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
                }
                else
                {
                    var data = _statisticService.GetGetRevenueStatistic(fromDate, toDate).ToList();
                    await ReportHelper.GenerateXls(data, fullPath);
                    return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
                }
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
