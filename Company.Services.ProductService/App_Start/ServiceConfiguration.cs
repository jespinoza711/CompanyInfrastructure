using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company.Services.ProductService
{
    public class ServiceConfiguration
    {
        private IDependencyResolver dependencyResolver;

        public IDependencyResolver DependencyResolver
        {
            get
            {
                return this.dependencyResolver;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.dependencyResolver = value;
            }
        }
    }

    public static class GlobalConfiguration
    {
        private static Lazy<ServiceConfiguration> configuration = new Lazy<ServiceConfiguration>(() => 
        {
            return new ServiceConfiguration();
        });

        public static ServiceConfiguration Configuration { get { return GlobalConfiguration.configuration.Value; } }
    }
}