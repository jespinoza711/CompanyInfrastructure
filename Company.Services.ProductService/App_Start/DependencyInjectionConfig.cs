using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Company.DataAccessLayer;
using Company.DataAccessLayer.Infrastructure;
using Company.Models.Production;
using Ninject;

namespace Company.Services.ProductService
{
    public static class DependencyInjectionConfig
    {
        public static void Register(ServiceConfiguration serviceConfiguration)
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            serviceConfiguration.DependencyResolver = new NinjectDependencyResolver(kernel);            
        }

        private static void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<IObjectContextProvider>().ToConstant<ObjectContextProvider>(new ObjectContextProvider());
            kernel.Bind<ICoreUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IRepository<Product>>().To<GenericRepository<Product>>();
        }
    }
}