using System.Collections.Generic;
using System.Linq;

namespace Company.Infrastructure.AppConfiguration
{
    public static class IDependencyScopeExtensions
    {
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// 
        /// <returns>
        /// The retrieved service.
        /// </returns>
        /// <typeparam name="TService">The service to be retrieved.</typeparam>
        public static TService GetService<TService>(this IDependencyScope scope)
        {
            return (TService)scope.GetService(typeof(TService));
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// 
        /// <returns>
        /// The retrieved collection of services.
        /// </returns>
        /// <typeparam name="TService">The collection of services to be retrieved.</typeparam>
        public static IEnumerable<TService> GetServices<TService>(this IDependencyScope scope)
        {
            return scope.GetServices(typeof(TService)).OfType<TService>();
        }
    }
}
