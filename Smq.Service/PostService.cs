using Smq.Common;
using Smq.Data.Infrastructure;
using Smq.Data.Repositories;
using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Service
{
    public interface IPostService
    {
        Post Add(Post post);
        void Update(Post post);
        Post Delete(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow);
        IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);
        Post GetById(int id);
        IEnumerable<Post> GetAllByTagPaging(string tag,int page, int pageSize, out int totalRow);
        IEnumerable<Post> GetAll(string keyword);
        IEnumerable<Post> GetPostById(int id, int page, int pageSize, out int totalRow);
        IEnumerable<Tag> GetAllTagPost();
        Tag GetTagPost(string tagId);
        void SaveChanges();

    }
    public class PostService:IPostService
    {
        IPostRepository _postRepository;
        IUnitOfWork _unitOfWork;
        ITagRepository _tagRepository;
        IPostTagRepository _postTagRepository;
        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork, ITagRepository tagRepository, IPostTagRepository postTagRepository)
        {
            this._postRepository = postRepository;
            this._unitOfWork = unitOfWork;
            this._tagRepository = tagRepository;
            this._postTagRepository = postTagRepository;
        }
        public Post Add(Post post)
        {
            var postAdd = _postRepository.Add(post);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(post.Tags))
            {
                string[] tags = post.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(n => n.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.PostTag;
                        _tagRepository.Add(tag);
                    }

                    PostTag postTag = new PostTag();
                    postTag.PostID = post.ID;
                    postTag.TagID = tagId;
                    _postTagRepository.Add(postTag);
                }
            }
            return post;
        }

        public void Update(Post post)
        {
            _postRepository.Update(post);
            if (!string.IsNullOrEmpty(post.Tags))
            {
                string[] tags = post.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(n => n.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.PostTag;
                        _tagRepository.Add(tag);
                    }
                    _postTagRepository.DeleteMulti(n => n.PostID == post.ID);
                    PostTag postTag = new PostTag();
                    postTag.PostID = post.ID;
                    postTag.TagID = tagId;
                    _postTagRepository.Add(postTag);
                }
            }
        }

        public Post Delete(int id)
        {
           return _postRepository.Delete(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll(new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            //Select all post by tag
            //return _postRepository.GetMultiPaging(n => n.Status, out totalRow, page, pageSize);
            var query = _postRepository.GetMulti(n => n.Status);
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Post GetById(int id)
        {
            return _postRepository.GetSingleById(id);
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag,int page, int pageSize, out int totalRow)
        {
            //Select all post by tag
            return _postRepository.GetAllByTag(tag, page, pageSize,out totalRow);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }


        public IEnumerable<Post> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _postRepository.GetMultiPaging(n => n.Status && n.CategoryID == categoryId, out totalRow, page, pageSize, new string[] { "PostCategory" });
        }

        public IEnumerable<Post> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _postRepository.GetMulti(n => n.Name.Contains(keyword) || n.Description.Contains(keyword));
            }
            else
            {
                return _postRepository.GetAll();
            }
        }

        public IEnumerable<Post> GetPostById(int id, int page, int pageSize, out int totalRow)
        {
            var query = _postRepository.GetMulti(n => n.Status && n.CategoryID == id);
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public IEnumerable<Tag> GetAllTagPost()
        {         
            var listTag = _tagRepository.GetAllTagPost();
            return listTag;
        }
        public Tag GetTagPost(string tagId)
        {
            return _tagRepository.GetSingleByCondition(n => n.ID == tagId);
        }
    }
}
