using System;
using System.Linq;
using System.Collections.Generic;
using Company.Infrastructure.BusinessLayer;
using Company.Infrastructure.DataAccessLayer;
using Company.Infrastructure.Domain;
using AutoMapper;

namespace Company.BusinessLayer
{
    public class DbProductLogic : IProductLogic
    {
        private readonly IRepository<Company.Models.Production.Product> productRepository;

        public DbProductLogic(IRepository<Company.Models.Production.Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        #region IProductLogic Members

        public IEnumerable<int> GetProductIds()
        {
            return this.productRepository.GetQuery().Select(p => p.ProductID).ToArray();
        }

        public Product GetProductById(int id)
        {
            return Mapper.Map<Product>(this.productRepository.Find(id));
        }

        #endregion
    }
}
