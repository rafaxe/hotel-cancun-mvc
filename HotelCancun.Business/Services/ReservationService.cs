using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCancun.Business.Services
{
    public class ReservationService : BaseService, IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository,
                              INotifier notifier) : base(notifier)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<Reservation> Add(Reservation reservation)
        {
            reservation.RecalculatePrice();
            
            if(!await CheckValidDate(reservation))
                return null;

            if (!ExecuteValidation(new ReservationValidation(), reservation)) return null;
            await _reservationRepository.Add(reservation);

            return reservation;
        }

        private async Task<bool> CheckValidDate(Reservation reservation)
        {
            var alreadyTaken = await _reservationRepository.GetReservationBySuiteDate(
                reservation.SuiteId, reservation.CheckIn, reservation.CheckOut
            );

            var reservations = alreadyTaken.ToList();

            if ((reservations.Any() && reservations.FirstOrDefault(x => x.Id == reservation.Id) == null))
            {
                Notify("There is already a reservation for this period");
                return false;
            }

            if((reservation.CheckIn - DateTime.Now).TotalDays > 30)
            {
                Notify("It can't be reserved more than 30 days in advance");
                return false;
            }

            return true;
        }

        public async Task Update(Reservation reservation)
        {
            reservation.RecalculatePrice();

            if (!await CheckValidDate(reservation))
                return;

            if (!ExecuteValidation(new ReservationValidation(), reservation)) return;
            await _reservationRepository.Update(reservation);
        }

        public async Task Remove(Guid id)
        {
            await _reservationRepository.Remove(id);
        }

        public void Dispose()
        {
            _reservationRepository?.Dispose();
        }
    }
}