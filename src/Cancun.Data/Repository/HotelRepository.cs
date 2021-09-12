using System;
using System.Threading.Tasks;
using Cancun.Business.Intefaces;
using Cancun.Business.Models;
using Cancun.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Data.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        public HotelRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Hotel> GetHotelAddress(Guid id)
        {
            return await Db.Hotels.AsNoTracking()
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Hotel> GetHotelSuitesAddress(Guid id)
        {
            return await Db.Hotels.AsNoTracking()
                .Include(c => c.Suites)
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}