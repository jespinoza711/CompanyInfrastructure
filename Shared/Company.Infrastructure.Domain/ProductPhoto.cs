using System;
using System.Collections.Generic;

namespace Company.Infrastructure.Domain
{
    public class ProductPhoto
    {
        public ProductPhoto()
        {
            this.ProductProductPhoto = new HashSet<ProductProductPhoto>();
        }

        public int ProductPhotoID { get; set; }
        public byte[] ThumbNailPhoto { get; set; }
        public string ThumbnailPhotoFileName { get; set; }
        public byte[] LargePhoto { get; set; }
        public string LargePhotoFileName { get; set; }

        public virtual ICollection<ProductProductPhoto> ProductProductPhoto { get; set; }
    }
}
