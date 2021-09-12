using System;
using System.Threading.Tasks;
using Cancun.Business.Models;

namespace Cancun.Business.Intefaces
{
    public interface IReservationService : IDisposable
    {
        Task Add(Reservation reservation);
        Task Update(Reservation reservation);
        Task Remove(Guid id);
    }
}