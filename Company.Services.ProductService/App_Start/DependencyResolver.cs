using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Syntax;

namespace Company.Services.ProductService
{
    /// <summary>
    /// Represents a dependency injection container.
    /// </summary>
    public interface IDependencyResolver : IDependencyScope, IDisposable
    {
        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// 
        /// <returns>
        /// The dependency scope.
        /// </returns>
        IDependencyScope BeginScope();
    }

    /// <summary>
    /// Represents an interface for the range of the dependencies.
    /// </summary>
    public interface IDependencyScope : IDisposable
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// 
        /// <returns>
        /// The retrieved service.
        /// </returns>
        /// <param name="serviceType">The service to be retrieved.</param>
        object GetService(Type serviceType);

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// 
        /// <returns>
        /// The retrieved collection of services.
        /// </returns>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        IEnumerable<object> GetServices(Type serviceType);
    }

    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.GetAll(serviceType);
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    }

    public static class IDependencyResolverExtensions
    {
        public static TService GetService<TService>(this IDependencyScope resolver)
        {
            return (TService)resolver.GetService(typeof(TService));
        }

        public static IEnumerable<TService> GetServices<TService>(this IDependencyScope resolver)
        {
            return resolver.GetServices(typeof(TService)).OfType<TService>();
        }
    }
}