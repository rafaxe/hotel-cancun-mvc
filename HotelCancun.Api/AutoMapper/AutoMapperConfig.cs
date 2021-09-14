using AutoMapper;
using HotelCancun.Api.Data;
using HotelCancun.Api.ViewModels;
using HotelCancun.Business.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelCancun.Api.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Hotel, HotelViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<Suite, SuiteViewModel>().ReverseMap();
            CreateMap<Reservation, ReservationViewModel>().ReverseMap();
            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
        }
    }
}