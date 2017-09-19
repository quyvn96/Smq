using Smq.Common;
using Smq.Data.Infrastructure;
using Smq.Data.Repositories;
using Smq.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace Smq.Service
{
    public interface IProductService
    {
        Product Add(Product Product);

        void Update(Product Product);

        Product Delete(int id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(string keyword);

        IEnumerable<Product> GetLastest(int top);

        IEnumerable<Product> GetHotProduct(int top);

        IEnumerable<Product> GetListProductCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> GetReatedProducts(int id, int top);
        IEnumerable<string> GetListProductByName(string name);

        Product GetById(int id);

        void Save();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;
        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._tagRepository = tagRepository;
            this._unitOfWork = unitOfWork;
        }

        public Product Add(Product Product)
        {
            var product = _productRepository.Add(Product);
            _unitOfWork.Commit();
            if (!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(n => n.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }

                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }
            return product;
        }

        public Product Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Product Product)
        {
            _productRepository.Update(Product);
            if (!string.IsNullOrEmpty(Product.Tags))
            {
                string[] tags = Product.Tags.Split(',');
                for (var i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    if (_tagRepository.Count(n => n.ID == tagId) == 0)
                    {
                        Tag tag = new Tag();
                        tag.ID = tagId;
                        tag.Name = tags[i];
                        tag.Type = CommonConstants.ProductTag;
                        _tagRepository.Add(tag);
                    }
                    _productTagRepository.DeleteMulti(n => n.ProductID == Product.ID);
                    ProductTag productTag = new ProductTag();
                    productTag.ProductID = Product.ID;
                    productTag.TagID = tagId;
                    _productTagRepository.Add(productTag);
                }
            }
        }

        public IEnumerable<Product> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productRepository.GetMulti(n => n.Name.Contains(keyword) || n.Description.Contains(keyword));
            }
            else
            {
                return _productRepository.GetAll();
            }
        }

        public IEnumerable<Product> GetLastest(int top)
        {
            return _productRepository.GetMulti(n => n.Status).OrderByDescending(n => n.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _productRepository.GetMulti(n => n.Status && n.HotFlag == true).OrderByDescending(n => n.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListProductCategoryIdPaging(int categoryId, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(n => n.Status && n.CategoryID == categoryId);
            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(n => n.ViewCount);
                    break;

                case "discount":
                    query = query.OrderByDescending(n => n.PromotionPrice.HasValue);
                    break;

                case "price":
                    query = query.OrderBy(n => n.Price);
                    break;

                default:
                    query = query.OrderByDescending(n => n.CreatedDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _productRepository.GetMulti(n => n.Status && n.Name.Contains(name)).Select(y => y.Name);
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(n => n.Status && n.Name.Contains(keyword));
            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(n => n.ViewCount);
                    break;

                case "discount":
                    query = query.OrderByDescending(n => n.PromotionPrice.HasValue);
                    break;

                case "price":
                    query = query.OrderBy(n => n.Price);
                    break;

                default:
                    query = query.OrderByDescending(n => n.CreatedDate);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }


        public IEnumerable<Product> GetReatedProducts(int id, int top)
        {
            var product = _productRepository.GetSingleById(id);
            return _productRepository.GetMulti(n => n.Status && n.ID != id && n.CategoryID == product.CategoryID).OrderByDescending(n => n.CreatedDate).Take(top);
        }
    }
}