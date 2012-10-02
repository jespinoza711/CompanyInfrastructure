using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using Company.Infrastructure.AppConfiguration;

namespace Company.Infrastructure.Ninject.Wcf
{
    public class DependencyInjectionBasedInstanceProvider : IInstanceProvider
    {
        private IDependencyResolver resolver;
        private Type serviceType;

        public DependencyInjectionBasedInstanceProvider(IDependencyResolver resolver, Type serviceType)
        {
            this.resolver = resolver;
            this.serviceType = serviceType;
        }

        #region IInstanceProvider Members

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            var service = this.resolver.GetService(this.serviceType);

            if (service == null)
            {
                throw new ServiceInjectionException("The dependency resolver was unable to resolve all dependencies on the type '" + this.serviceType.FullName + "'." +
                    Environment.NewLine +
                    "Possible reasons are: " + Environment.NewLine +
                    "Not all interfaces in the constructor of '" + this.serviceType.Name + "' where configured to be resolvable.")
                    {
                        UsedDependencyResolver = this.resolver
                    };
            }

            return service;
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance)
        {
        }

        #endregion
    }

    
}
