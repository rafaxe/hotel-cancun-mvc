using AutoMapper;
using Cancun.App.ViewModels;
using Cancun.Business.Models;

namespace Cancun.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Hotel, HotelViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<Suite, SuiteViewModel>().ReverseMap();
            CreateMap<Reservation, ReservationViewModel>().ReverseMap();
        }
    }
}