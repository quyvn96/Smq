using Smq.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Smq.Service;
using AutoMapper;
using Smq.Model.Models;
using Smq.Web.Models;
using System.Web.Script.Serialization;
using Smq.Web.Infrastructure.Extensions;

namespace Smq.Web.Api
{
    [RoutePrefix("api/post")]
    public class PostController : ApiControllerBase
    {
        private IPostService _postService;
        public PostController(IErrorService errorService,IPostService postService) : base(errorService)
        {
            this._postService = postService;
        }
        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _postService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(model);

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
                var model = _postService.GetById(id);

                var responseData = Mapper.Map<Post, PostViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _postService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(query);

                var paginationSet = new PaginationSet<PostViewModel>()
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


        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, PostViewModel postCategoryVm)
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
                    var newPost = new Post();
                    newPost.UpdatePost(postCategoryVm);
                    newPost.CreatedDate = DateTime.Now;
                    newPost.CreatedBy = User.Identity.Name;
                    _postService.Add(newPost);
                    _postService.SaveChanges();

                    var responseData = Mapper.Map<Post, PostViewModel>(newPost);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, PostViewModel postVm)
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
                    var dbPost = _postService.GetById(postVm.ID);

                    dbPost.UpdatePost(postVm);
                    dbPost.UpdatedDate = DateTime.Now;
                    dbPost.UpdatedBy = User.Identity.Name;
                    _postService.Update(dbPost);
                    _postService.SaveChanges();

                    var responseData = Mapper.Map<Post, PostViewModel>(dbPost);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
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
                    var oldPostCategory = _postService.Delete(id);
                    _postService.SaveChanges();

                    var responseData = Mapper.Map<Post, PostViewModel>(oldPostCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
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
                    var listPostCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listPostCategory)
                    {
                        _postService.Delete(item);
                    }

                    _postService.SaveChanges();

                    response = request.CreateResponse(HttpStatusCode.OK, listPostCategory.Count);
                }

                return response;
            });
        }
    }
}
