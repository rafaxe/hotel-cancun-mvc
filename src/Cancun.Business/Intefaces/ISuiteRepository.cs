using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cancun.Business.Models;

namespace Cancun.Business.Intefaces
{
    public interface ISuiteRepository : IRepository<Suite>
    {
        Task<IEnumerable<Suite>> GetSuitesByHotel(Guid hotelId);
        Task<IEnumerable<Suite>> GetSuitesHotels();
        Task<Suite> GetSuiteHotel(Guid id);
    }
}