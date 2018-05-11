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
    [RoutePrefix("api/order")]
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        private IOrderService _orderService;
        private IProductService _productService;

        public OrderController(IErrorService errorService, IOrderService orderService, IProductService productService)
            : base(errorService)
        {
            this._orderService = orderService;
            this._productService = productService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query);

                var paginationSet = new PaginationSet<OrderViewModel>()
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
                    var orderObject = _orderService.DeleteOrder(id);
                    _orderService.Save();

                    var responseData = orderObject;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("getorderdetailbyid")]
        [HttpGet]
        public HttpResponseMessage GetOrderDetailById(HttpRequestMessage request, int id, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetOrderDetailById(id);
                var listProduct = _productService.GetAll();
                var listOrderVM = new List<OrderDetailViewModel>();
                foreach (var itemOrder in model)
                {
                    foreach (var itemProduct in listProduct)
                    {
                        if (itemProduct.ID == itemOrder.ProductID)
                        {
                            listOrderVM.Add(new OrderDetailViewModel
                            {
                                OrderID = itemOrder.OrderID,
                                ProductID = itemOrder.ProductID,
                                ProductName = itemProduct.Name,
                                Quantity = itemOrder.Quantity,
                                Price = itemOrder.Price,
                                TotalMoney = (itemOrder.Price * itemOrder.Quantity)
                            });
                        }
                    }
                }
                var totalQuantity = listOrderVM.Sum(n => n.Quantity);
                var total = listOrderVM.Sum(n => n.Quantity * n.Price);
                var totalRow = listOrderVM.Count;
                var paginationSet = new PaginationSet<OrderDetailViewModel>()
                {
                    Items = listOrderVM,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    TotalQuantity = totalQuantity,
                    Total = total
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }
        [HttpGet]
        [Route("exportexcel")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, int id)
        {
            string fileName = string.Concat("OrderDetail_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _orderService.GetAll(null).ToList();
                var model = _orderService.GetOrderDetailById(id);
                var listProduct = _productService.GetAll();
                var listOrderVM = new List<OrderDetailViewModel>();
                foreach (var itemOrder in model)
                {
                    foreach (var itemProduct in listProduct)
                    {
                        if (itemProduct.ID == itemOrder.ProductID)
                        {
                            listOrderVM.Add(new OrderDetailViewModel
                            {
                                OrderID = itemOrder.OrderID,
                                ProductID = itemOrder.ProductID,
                                ProductName = itemProduct.Name,
                                Quantity = itemOrder.Quantity,
                                Price = itemOrder.Price,
                                TotalMoney = (itemOrder.Price * itemOrder.Quantity)
                            });
                        }
                    }
                }
                var listOrderNewVM = new List<OrderDetailViewModel>();
                foreach (var item in data)
                {
                    foreach (var itemDetail in listOrderVM)
                    {
                        if (item.ID == itemDetail.OrderID)
                        {
                            listOrderNewVM.Add(new OrderDetailViewModel
                            {
                                CustomerAddress = item.CustomerAddress,
                                CustomerName = item.CustomerName,
                                OrderID = itemDetail.OrderID,
                                ProductID = itemDetail.ProductID,
                                ProductName = itemDetail.ProductName,
                                Quantity = itemDetail.Quantity,
                                Price = itemDetail.Price,
                                TotalMoney = itemDetail.TotalMoney
                            });
                        }
                    }
                }
                var dataChild = listOrderNewVM;
                await ReportHelper.GenerateXls(dataChild, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [Route("deleteorderdetails")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrderDetails(HttpRequestMessage request, int orderId, int productId)
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
                    var orderObject = _orderService.DeleteOrderDetails(orderId, productId);
                    _orderService.Save();

                    var responseData = orderObject;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("updateorderdetails")]
        [HttpGet]
        public HttpResponseMessage UpdateOrderDetails(HttpRequestMessage request, int orderId, int productId, int quantity)
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
                    var orderObject = _orderService.UpdateOrderDetails(orderId, productId, quantity);
                    _orderService.Save();

                    var responseData = orderObject;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("getquantity")]
        [HttpGet]
        public HttpResponseMessage UpdateOrderDetails(HttpRequestMessage request, int orderId, int productId)
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
                    var orderObject = _orderService.GetAllOrderDetail();
                    _orderService.Save();

                    var responseData = orderObject.Where(n=>n.OrderID == orderId && n.ProductID == productId).Select(x=>x.Quantity).ToList();
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("updateorderstatus")]
        [HttpGet]
        public HttpResponseMessage DeleteOrder(HttpRequestMessage request, int id, bool status)
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
                    var orderObject = _orderService.UpdateOrderStatus(id, status);
                    _orderService.Save();

                    var responseData = orderObject;
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
    }
}
