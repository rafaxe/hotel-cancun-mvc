using System;
using System.Threading.Tasks;
using Cancun.Business.Models;

namespace Cancun.Business.Intefaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Hotel> GetHotelAddress(Guid id);
        Task<Hotel> GetHotelSuitesAddress(Guid id);
    }
}