using System;
using Company.Infrastructure.AppConfiguration;
using Company.Services.Wcf.Products.Configuration;

namespace Company.Services.ProductService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutomapperConfiguration.Configure();
            DependencyInjectionConfig.Register(GlobalConfiguration.Configuration);            
        }
    }
}