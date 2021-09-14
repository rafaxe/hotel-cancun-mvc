﻿using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Business.Models.Validations;
using System;
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

        public async Task Add(Reservation reservation)
        {
            reservation.RecalculatePrice();
            if (!ExecuteValidation(new ReservationValidation(), reservation)) return;
            await _reservationRepository.Add(reservation);
        }

        public async Task Update(Reservation reservation)
        {
            reservation.RecalculatePrice();
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