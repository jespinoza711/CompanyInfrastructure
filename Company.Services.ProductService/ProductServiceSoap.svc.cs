using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Company.Services.ProductService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductServiceSoap" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductServiceSoap.svc or ProductServiceSoap.svc.cs at the Solution Explorer and start debugging.
    public class ProductServiceSoap : IProductServiceSoap
    {

        #region IProductServiceSoap Members

        public GetSomethingForProductsResponse GetSomethingForProducts(GetSomethingForProductsRequest request)
        {        

            Company.DataAccessLayer.Infrastructure.IRepository<Company.Models.Production.Product> productsRepository =
                GlobalConfiguration.Configuration.DependencyResolver.GetService<Company.DataAccessLayer.Infrastructure.IRepository<Company.Models.Production.Product>>();

            var products = productsRepository.Find(2);

            

            throw new NotImplementedException();
        }

        #endregion
    }
}
