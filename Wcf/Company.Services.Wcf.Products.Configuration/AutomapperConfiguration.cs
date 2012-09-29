using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Services.Wcf.Products.Configuration
{
    public static class AutomapperConfiguration
    {
        public static void Configure()
        {
            Company.BusinessLayer.AutomapperConfiguration.Configure();
        }
    }
}
