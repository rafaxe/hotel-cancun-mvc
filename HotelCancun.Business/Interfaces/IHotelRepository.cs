using System;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Hotel> GetHotelAddress(Guid id);
        Task<Hotel> GetHotelSuitesAddress(Guid id);
    }
}