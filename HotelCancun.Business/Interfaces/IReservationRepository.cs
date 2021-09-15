using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelCancun.Business.Models;

namespace HotelCancun.Business.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetMonthDates(DateTime dateMonth);
        Task<Reservation> GetReservation(Guid id);
        Task<IEnumerable<Reservation>> GetReservationsSuites();
        Task<IEnumerable<Reservation>> GetReservationByUserId(string userId);
        Task<IEnumerable<Reservation>> GetReservationBySuiteDate(Guid suiteId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<Reservation>> GetReservationBySuite(Guid suiteId);
        Task<IEnumerable<Reservation>> GetReservationByHotel(Guid hotelId);
    }
}