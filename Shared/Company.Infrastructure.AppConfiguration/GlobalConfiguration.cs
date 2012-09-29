using System;

namespace Company.Infrastructure.AppConfiguration
{
    public static class GlobalConfiguration
    {
        private static Lazy<AppConfiguration> configuration = new Lazy<AppConfiguration>(() =>
        {
            return new AppConfiguration();
        });

        public static AppConfiguration Configuration { get { return GlobalConfiguration.configuration.Value; } }
    }
}
