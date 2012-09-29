using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Infrastructure.AppConfiguration
{
    public class AppConfiguration : IDisposable
    {
        private List<IDisposable> resourcesToDispose;
        private IDependencyResolver dependencyResolver;
        private Action<AppConfiguration> initializer;
        private ConcurrentDictionary<object, object> properties;
        private bool disposed;

        public AppConfiguration()
        {
            this.resourcesToDispose = new List<IDisposable>();
            this.properties = new ConcurrentDictionary<object, object>();
        }

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

        public Action<AppConfiguration> Initializer
        {
            get
            {
                return this.initializer;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.initializer = value;
            }
        }

        public void RegisterForDispose(IDisposable resource)
        {
            if (resource == null)
                return;
            this.resourcesToDispose.Add(resource);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            this.disposed = true;
            if (!disposing)
                return;
            this.dependencyResolver.Dispose();
            foreach (IDisposable disposable in this.resourcesToDispose)
                disposable.Dispose();
        }

        #endregion
    }    
}
