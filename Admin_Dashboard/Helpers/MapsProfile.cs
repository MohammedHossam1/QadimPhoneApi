using Admin_Dashboard.Models;
using AutoMapper;
using Talabat.Core.Entities;

namespace Admin_Dashboard.Helpers
{
    public class MapsProfile : Profile
    {
        public MapsProfile()
        {
            CreateMap<Product , ProductViewModel>().ReverseMap();
        }
    }
}
