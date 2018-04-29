using AutoMapper;
using Smq.Model.Models;
using Smq.Service;
using Smq.Web.Infrastructure.Core;
using Smq.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Smq.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;

namespace Smq.Web.Api
{
    [RoutePrefix("api/postcategory")]
    [Authorize]
    public class PostCategoryController : ApiControllerBase
    {
        IPostCategoryService _postCategoryService;
        public PostCategoryController(IErrorService errorService, IPostCategoryService postCategoryService)
            : base(errorService)
        {
            this._postCategoryService = postCategoryService;
        }
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request,string keyword,int page,int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var listCategory =  _postCategoryService.GetAll(keyword);
                totalRow = listCategory.Count();
                var query = listCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var listPostCategoryVm = Mapper.Map<IEnumerable<PostCategory>,IEnumerable<PostCategoryViewModel>>(query);
                var paginationSet = new PaginationSet<PostCategoryViewModel>()
                {
                    Items = listPostCategoryVm,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }
        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _postCategoryService.GetAll();

                var responseData = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _postCategoryService.GetById(id);

                var responseData = Mapper.Map<PostCategory, PostCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        //Post
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
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
                    var newPostCategory = new PostCategory();
                    newPostCategory.UpdatePostCategory(postCategoryVm);
                    newPostCategory.CreatedDate = DateTime.Now;
                    newPostCategory.CreatedBy = User.Identity.Name;
                    _postCategoryService.Add(newPostCategory);
                    _postCategoryService.Save();

                    var responseData = Mapper.Map<PostCategory, PostCategoryViewModel>(newPostCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        //Put
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, PostCategoryViewModel postCategoryVm)
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
                    var dbPostCategory = _postCategoryService.GetById(postCategoryVm.ID);
                    dbPostCategory.UpdatePostCategory(postCategoryVm);
                    dbPostCategory.UpdatedDate = DateTime.Now;
                    dbPostCategory.CreatedBy = User.Identity.Name;
                    _postCategoryService.Update(dbPostCategory);
                    _postCategoryService.Save();

                    var responseData = Mapper.Map<PostCategory, PostCategoryViewModel>(dbPostCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        //delete
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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
                    var oldProductCategory = _postCategoryService.Delete(id);
                    _postCategoryService.Save();

                    var responseData = Mapper.Map<PostCategory, PostCategoryViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedPostCategories)
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
                    var listPostCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedPostCategories);
                    foreach (var item in listPostCategory)
                    {
                        _postCategoryService.Delete(item);
                    }
                    _postCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, listPostCategory.Count);
                }

                return response;
            });
        }
    }
}