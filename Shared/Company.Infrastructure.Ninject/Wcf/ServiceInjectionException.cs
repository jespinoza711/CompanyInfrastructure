using System;
using Company.Infrastructure.AppConfiguration;

namespace Company.Infrastructure.Ninject.Wcf
{
    [Serializable]
    public sealed class ServiceInjectionException : Exception
    {
        public ServiceInjectionException() { }
        public ServiceInjectionException(string message) : base(message) { }
        public ServiceInjectionException(string message, Exception inner) : base(message, inner) { }
        protected ServiceInjectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public IDependencyResolver UsedDependencyResolver { get; internal set; }
    }
}
