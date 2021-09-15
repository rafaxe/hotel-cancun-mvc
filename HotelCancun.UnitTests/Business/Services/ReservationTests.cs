using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Business.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelCancun.UnitTests.Business.Services
{
    public class ReservationTests
    {
        [Fact]
        [DisplayName("The reservation should be added with success")]
        public async Task ShouldBeAddedWithSuccess()
        {
            // Arrange
            var repository = new Mock<IReservationRepository>();
            var notifier = new Mock<INotifier>();
            
            var reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                CheckIn = DateTime.Now,
                CheckOut = DateTime.Now.AddDays(1),
                ApplicationUserId = Guid.NewGuid().ToString(),
                Days = 1,
                PriceTotal = 0,
                Suite = new Suite()
                {
                    Price = 20
                },
                SuiteId = Guid.NewGuid(),
            };

            repository.Setup(x => x.GetReservationBySuiteDate(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Reservation>());

            var service = new ReservationService(repository.Object, notifier.Object);

            // Act
            var result = await service.Add(reservation);

            // Assert
            Assert.Equal(20, result.PriceTotal);
            Assert.Equal(reservation.CheckIn, result.CheckIn);
            Assert.Equal(reservation.CheckOut, result.CheckOut);
            Assert.Equal(1, result.Days);
        }

        [Fact]
        [DisplayName("Should not save because of max days limitation")]
        public async Task ShouldNotSaveBecauseOfMaxDaysLimitation()
        {
            // Arrange
            var repository = new Mock<IReservationRepository>();
            var notifier = new Mock<INotifier>();

            var reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                CheckIn = DateTime.Now,
                CheckOut = DateTime.Now.AddDays(4),
                ApplicationUserId = Guid.NewGuid().ToString(),
                Days = 4,
                PriceTotal = 0,
                Suite = new Suite()
                {
                    Price = 20
                },
                SuiteId = Guid.NewGuid(),
            };

            repository.Setup(x => x.GetReservationBySuiteDate(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync(new List<Reservation>());

            var service = new ReservationService(repository.Object, notifier.Object);

            // Act
            var result = await service.Add(reservation);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [DisplayName("Should not save because of max anticipation  days limitation")]
        public async Task ShouldNotSaveBecauseOfMaxDaysAnticipationDaysLimitation()
        {
            // Arrange
            var repository = new Mock<IReservationRepository>();
            var notifier = new Mock<INotifier>();

            var reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                CheckIn = DateTime.Now.AddDays(31),
                CheckOut = DateTime.Now.AddDays(32),
                ApplicationUserId = Guid.NewGuid().ToString(),
                Days = 1,
                PriceTotal = 0,
                Suite = new Suite()
                {
                    Price = 20
                },
                SuiteId = Guid.NewGuid(),
            };

            repository.Setup(x => x.GetReservationBySuiteDate(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Reservation>());

            var service = new ReservationService(repository.Object, notifier.Object);

            // Act
            var result = await service.Add(reservation);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        [DisplayName("Should not save because of a day conflict")]
        public async Task ShouldNotSaveBecauseOfADayConflict()
        {

            // Arrange
            var repository = new Mock<IReservationRepository>();
            var notifier = new Mock<INotifier>();

            var reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                CheckIn = DateTime.Now.Date,
                CheckOut = DateTime.Now.Date.AddDays(1),
                ApplicationUserId = Guid.NewGuid().ToString(),
                Days = 1,
                PriceTotal = 0,
                Suite = new Suite()
                {
                    Price = 20
                },
                SuiteId = Guid.NewGuid(),
            };

            var conflictReservations = new List<Reservation>
            {
                new ()
                {
                    Id = Guid.NewGuid(),
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddDays(2),
                    ApplicationUserId = Guid.NewGuid().ToString(),
                    Days = 2,
                    PriceTotal = 0,
                    Suite = new Suite()
                    {
                        Price = 20
                    },
                    SuiteId = Guid.NewGuid(),
                }
            }.ToList();

            repository.Setup(x => x.GetReservationBySuiteDate(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(conflictReservations);


            var service = new ReservationService(repository.Object, notifier.Object);

            // Act
            var result = await service.Add(reservation);

            // Assert
            Assert.Null(result);
        }
    }
}
