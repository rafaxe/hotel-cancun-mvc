using System;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface ISuiteService : IDisposable
    {
        Task Add(Suite suite);
        Task Update(Suite suite);
        Task Remove(Guid id);
    }
}