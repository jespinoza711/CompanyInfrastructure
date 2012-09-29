using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Infrastructure.Domain
{
    public class ProductProductPhoto
    {
        public int ProductID { get; set; }
        public int ProductPhotoID { get; set; }
        public bool Primary { get; set; }

        public Product Product { get; set; }
        public ProductPhoto ProductPhoto { get; set; }
    }
}
