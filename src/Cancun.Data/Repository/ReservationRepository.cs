using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cancun.Business.Intefaces;
using Cancun.Business.Models;
using Cancun.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Data.Repository
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext context) : base(context) { }

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

        public async Task<IEnumerable<Reservation>> GetReservationByHotel(Guid hotelId)
        {
            return await Search(p => p.Suite.HotelId == hotelId);
        }

        public override async Task Add(Reservation entity)
        {
            Db.Entry(entity.Suite).State = EntityState.Unchanged;
            DbSet.Add(entity);
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