using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Infrastructure.Domain
{
    public class ProductModelIllustration
    {
        public int ProductModelID { get; set; }
        public int IllustrationID { get; set; }

        public ProductModel ProductModel { get; set; }
    }
}
