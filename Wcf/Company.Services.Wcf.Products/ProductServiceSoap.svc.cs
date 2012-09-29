using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Company.Infrastructure.AppConfiguration;
using Company.Infrastructure.BusinessLayer;



namespace Company.Services.ProductService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductServiceSoap" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductServiceSoap.svc or ProductServiceSoap.svc.cs at the Solution Explorer and start debugging.
    public class ProductServiceSoap : IProductServiceSoap
    {
        private IDependencyResolver resolver = GlobalConfiguration.Configuration.DependencyResolver;

        #region IProductServiceSoap Members

        public GetSomethingForProductsResponse GetSomethingForProducts(GetSomethingForProductsRequest request)
        {
            var logic = resolver.GetService<IProductLogic>();
            var ids = logic.GetProductIds();

            var products = logic.GetProductById(ids.First());
            throw new NotImplementedException();
        }

        #endregion
    }
}
