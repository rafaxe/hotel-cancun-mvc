using System;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface IReservationService : IDisposable
    {
        Task Add(Reservation reservation);
        Task Update(Reservation reservation);
        Task Remove(Guid id);
    }
}