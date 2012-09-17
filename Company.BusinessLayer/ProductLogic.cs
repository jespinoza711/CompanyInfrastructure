using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.BusinessLayer.Contracts;
using Company.BusinessLayer.Domain;

namespace Company.BusinessLayer
{
    public class ProductLogic : IProductLogic
    {
        #region IProductLogic Members

        public IEnumerable<int> GetProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
