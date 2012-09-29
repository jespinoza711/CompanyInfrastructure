using System;
using System.Collections.Generic;

namespace Company.Infrastructure.Domain
{
    public class ProductModel
    {
        public ProductModel()
        {
            this.Product = new HashSet<Product>();
            this.ProductModelIllustration = new HashSet<ProductModelIllustration>();
        }

        public int ProductModelID { get; set; }
        public string Name { get; set; }
        public string CatalogDescription { get; set; }
        public string Instructions { get; set; }

        public ICollection<Product> Product { get; set; }
        public ICollection<ProductModelIllustration> ProductModelIllustration { get; set; }
    }
}
