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