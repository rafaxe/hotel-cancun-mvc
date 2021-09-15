using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface ISuiteRepository : IRepository<Suite>
    {
        Task<IEnumerable<Suite>> GetSuitesByHotel(Guid hotelId);
        Task<IEnumerable<Suite>> GetSuites();
        Task<Suite> GetSuiteHotel(Guid id);
    }
}