using AutoMapper;
using Company.Infrastructure.Domain;

namespace Company.BusinessLayer.Automapper
{
    internal sealed class ProductModelIllustrationBusinessLayerProfile : Profile
    {
        public override string ProfileName { get { return this.GetType().Name; } }

        protected override void Configure()
        {
            CreateMap<Company.Models.Production.ProductModelIllustration, ProductModelIllustration>();
        }
    }
}
