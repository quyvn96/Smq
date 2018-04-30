using AutoMapper;
using Smq.Common;
using Smq.Model.Models;
using Smq.Service;
using Smq.Web.Infrastructure.Core;
using Smq.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smq.Web.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        IPostService _postService;
        ICommonService _commonService;
        IPostCategoryService _postCategoryService;
        public PostController(IPostService postService, ICommonService commonService, IPostCategoryService postCategoryService)
        {
            this._commonService = commonService;
            this._postService = postService;
            this._postCategoryService = postCategoryService;
        }
        [HttpGet]
        public ActionResult Index(int id, int page = 1)
        {
            ViewBag.PostTags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_postService.GetAllTagPost());
            var postCategory = _postCategoryService.GetById(id);
            var postCategoryVM = Mapper.Map<PostCategory, PostCategoryViewModel>(postCategory);
            ViewBag.PostCategory = postCategoryVM;
            int pageSize = 5;
            int totalRow = 0;
            var postModel = _postService.GetPostById(id,page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            var postViewModelFormat = new List<PostViewModel>();

            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }
        public ActionResult PostByTag(string tagId, int page = 1)
        {
            int pageSize = 5;
            int totalRow = 0;
            var postModel = _postService.GetAllByTagPaging(tagId, page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            ViewBag.TagPost = Mapper.Map<Tag, TagViewModel>(_postService.GetTagPost(tagId));
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }
    }
}