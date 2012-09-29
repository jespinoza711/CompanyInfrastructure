using AutoMapper;
using Company.Infrastructure.Domain;

namespace Company.BusinessLayer.Automapper
{
    internal sealed class ProductProductPhotoBusinessLayerProfile : Profile
    {
        public override string ProfileName { get { return this.GetType().Name; } }

        protected override void Configure()
        {
            CreateMap<Company.Models.Production.ProductProductPhoto, ProductProductPhoto>();
        }
    }
}
