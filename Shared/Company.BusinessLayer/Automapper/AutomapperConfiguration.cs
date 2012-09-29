using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Company.BusinessLayer
{
    public static class AutomapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(c =>
                {
                    c.AddProfile<Company.BusinessLayer.Automapper.ProductBusinessLayerProfile>();
                    c.AddProfile<Company.BusinessLayer.Automapper.ProductModelBusinessLayerProfile>();
                    c.AddProfile<Company.BusinessLayer.Automapper.ProductModelIllustrationBusinessLayerProfile>();
                    c.AddProfile<Company.BusinessLayer.Automapper.ProductPhotoBusinessLayerProfile>();
                    c.AddProfile<Company.BusinessLayer.Automapper.ProductProductPhotoBusinessLayerProfile>();
                });
        }
    }
}
