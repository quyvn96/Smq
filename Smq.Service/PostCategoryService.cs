using Smq.Data.Infrastructure;
using Smq.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smq.Model.Models;

namespace Smq.Service
{
    public interface IPostCategoryService
    {
        PostCategory Add(PostCategory postCategory);
        void Update(PostCategory postCategory);
        PostCategory Delete(int id);
        IEnumerable<PostCategory> GetAll();
        IEnumerable<PostCategory> GetAllByParentId(int parentId);
        PostCategory GetById(int id);
        IEnumerable<PostCategory> GetAll(string keyword);
        void Save();
      
    }
    public class PostCategoryService : IPostCategoryService
    {
        IPostCategoryRepository _postCategoryRepository;
        IUnitOfWork _unitOfWork;
        public PostCategoryService(IPostCategoryRepository postCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._postCategoryRepository = postCategoryRepository;
            this._unitOfWork = unitOfWork;
        }
        public PostCategory Add(PostCategory postCategory)
        {
           return _postCategoryRepository.Add(postCategory);
        }

        public void Update(PostCategory postCategory)
        {
            _postCategoryRepository.Update(postCategory);
        }

        public PostCategory Delete(int id)
        {
           return _postCategoryRepository.Delete(id);
        }

        public IEnumerable<PostCategory> GetAll()
        {
            return _postCategoryRepository.GetAll();
        }

        public IEnumerable<PostCategory> GetAllByParentId(int parentId)
        {
            return _postCategoryRepository.GetMulti(n => n.Status && n.ParentID == parentId);
        }

        public PostCategory GetById(int id)
        {
            return _postCategoryRepository.GetSingleById(id);
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<PostCategory> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _postCategoryRepository.GetMulti(n => n.Name.Contains(keyword) || n.Description.Contains(keyword));
            }
            else
            {
                return _postCategoryRepository.GetAll();
            }
        }
    }
}
