using Company.Infrastructure.AppConfiguration;
using Company.Infrastructure.DataAccessLayer;
using Company.Infrastructure.BusinessLayer;
using Company.Infrastructure.Ninject;
using Company.Models.Production;
using Ninject;

namespace Company.Services.Wcf.Products.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void Register(AppConfiguration serviceConfiguration)
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            serviceConfiguration.DependencyResolver = new NinjectDependencyResolver(kernel);
        }

        private static void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<IObjectContextProvider>().ToConstant<ObjectContextProvider>(new ObjectContextProvider());
            kernel.Bind<ICoreUnitOfWork>().To<Company.DataAccessLayer.UnitOfWork>();
            kernel.Bind<IRepository<Product>>().To<Company.DataAccessLayer.GenericRepository<Product>>();

            kernel.Bind<IProductLogic>().To<Company.BusinessLayer.DbProductLogic>();
        }
    }
}
