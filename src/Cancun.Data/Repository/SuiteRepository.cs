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
    public class SuiteRepository : Repository<Suite>, ISuiteRepository
    {
        public SuiteRepository(AppDbContext context) : base(context) { }

        public async Task<Suite> GetSuiteHotel(Guid id)
        {
            return await Db.Suites.AsNoTracking().Include(f => f.Hotel)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Suite>> GetSuitesHotels()
        {
            return await Db.Suites.AsNoTracking().Include(f => f.Hotel)
                .OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Suite>> GetSuitesByHotel(Guid hotelId)
        {
            return await Search(p => p.HotelId == hotelId);
        }
    }
}