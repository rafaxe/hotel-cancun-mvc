using System;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressByHotel(Guid hotelId);
    }
}