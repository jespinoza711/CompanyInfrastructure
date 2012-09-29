using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Infrastructure.Domain;

namespace Company.Infrastructure.BusinessLayer
{
    public interface IProductLogic
    {
        IEnumerable<int> GetProductIds();

        Product GetProductById(int id);
    }
}
