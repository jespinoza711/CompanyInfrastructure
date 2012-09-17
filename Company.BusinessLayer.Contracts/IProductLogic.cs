using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.BusinessLayer.Domain;

namespace Company.BusinessLayer.Contracts
{
    public interface IProductLogic
    {
        IEnumerable<int> GetProducts();

        IEnumerable<Product> GetProductById(int id);
    }
}
