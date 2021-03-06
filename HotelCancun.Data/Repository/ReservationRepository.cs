using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCancun.Data.Repository
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Reservation>> GetMonthDates(DateTime dateMonth)
        {
            return await Search(p => p.CheckIn.Month == dateMonth.Month || p.CheckOut.Month == dateMonth.Month);
        }

        public async Task<Reservation> GetReservation(Guid id)
        {
            return await Db.Reservations.AsNoTracking().Include(f => f.Suite)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsSuites()
        {
            return await Db.Reservations.AsNoTracking().Include(f => f.Suite)
                .OrderByDescending(p => p.CheckIn).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationByUserId(string userId)
        {
            return await Search(p => p.ApplicationUserId == userId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationBySuite(Guid suiteId)
        {
            return await Search(p => p.SuiteId == suiteId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationBySuiteDate(Guid suiteId, DateTime checkIn, DateTime checkOut)
        {
            return await Search(
                p => p.SuiteId == suiteId &&
                (checkIn <= p.CheckOut && checkIn >= p.CheckIn || p.CheckIn <= checkOut && p.CheckIn >= checkIn)
            );

        }

        public async Task<IEnumerable<Reservation>> GetReservationByHotel(Guid hotelId)
        {
            return await Search(p => p.Suite.HotelId == hotelId);
        }

        public override async Task Add(Reservation entity)
        {
            Db.Entry(entity.Suite).State = EntityState.Unchanged;
            await DbSet.AddAsync(entity);
            await SaveChanges();
        }

        public override async Task Update(Reservation entity)
        {
            Db.Entry(entity.Suite).State = EntityState.Unchanged;
            DbSet.Update(entity);
            await SaveChanges();
        }

        public override async Task Remove(Guid id)
        {
            var entity = new Reservation { Id = id };
            DbSet.Remove(entity);
            await SaveChanges();
        }
    }
}