using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(b => b.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(b=>b.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>());



            CreateMap<Product, PostProductDto>().ReverseMap();


        }


    }
}
