using Smq.Common.ViewModels;
using Smq.Data.Infrastructure;
using Smq.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.SqlClient;

namespace Smq.Data.Repositories
{
        public interface IProductRepository : IRepository<Product>
        {
            IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);
            IEnumerable<PriceFilterViewModel> GetPriceForFilter();
            IEnumerable<Product> GetProductByPrice(int id);
        }

        public class ProductRepository : RepositoryBase<Product>, IProductRepository
        {
            public ProductRepository(IDbFactory dbFactory): base(dbFactory)
            {
            }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
 	        var query = from p in DbContext.Products
                        join pt in DbContext.ProductTags
                        on p.ID equals pt.ProductID
                        where pt.TagID == tagId
                        select p;
            totalRow = query.Count();
            return query.OrderByDescending(n=>n.CreatedDate).Skip((page-1)*pageSize).Take(pageSize);
        }

        public IEnumerable<PriceFilterViewModel> GetPriceForFilter()
        {
            try
            {
                return DbContext.Database.SqlQuery<PriceFilterViewModel>("EXEC GetPriceForFilter").ToList();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                return new List<PriceFilterViewModel>();
            }
        }

        public IEnumerable<Product> GetProductByPrice(int id)
        {
            try
            {
                var parameters = new SqlParameter[]{
                new SqlParameter("@Price",id) };
                return DbContext.Database.SqlQuery<Product>("EXEC GetProductByPrice @Price", parameters).ToList();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return new List<Product>();
            }
        }
    }
}