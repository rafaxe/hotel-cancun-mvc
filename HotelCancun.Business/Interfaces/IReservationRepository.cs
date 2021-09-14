using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation> GetReservation(Guid id);
        Task<IEnumerable<Reservation>> GetReservationsSuites();
        Task<IEnumerable<Reservation>> GetReservationByUserId(string userId);
        Task<IEnumerable<Reservation>> GetReservationBySuite(Guid suiteId);
        Task<IEnumerable<Reservation>> GetReservationByHotel(Guid hotelId);
    }
}