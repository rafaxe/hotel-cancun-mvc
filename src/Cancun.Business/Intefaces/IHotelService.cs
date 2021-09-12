using System;
using System.Threading.Tasks;
using Cancun.Business.Models;

namespace Cancun.Business.Intefaces
{
    public interface IHotelService : IDisposable
    {
        Task Add(Hotel hotel);
        Task Update(Hotel hotel);
        Task Remove(Guid id);

        Task UpdateAddress(Address address);
    }
}