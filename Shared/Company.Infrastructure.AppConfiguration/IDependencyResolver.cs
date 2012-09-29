using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Infrastructure.AppConfiguration
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
}
