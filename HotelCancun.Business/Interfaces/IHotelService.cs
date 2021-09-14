using System;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface IHotelService : IDisposable
    {
        Task Add(Hotel hotel);
        Task Update(Hotel hotel);
        Task Remove(Guid id);

        Task UpdateAddress(Address address);
    }
}