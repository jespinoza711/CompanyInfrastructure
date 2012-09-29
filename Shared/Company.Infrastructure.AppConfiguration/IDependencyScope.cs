using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Infrastructure.AppConfiguration
{
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
}
