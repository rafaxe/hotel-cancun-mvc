using System;
using System.Threading.Tasks;
using Cancun.Business.Intefaces;
using Cancun.Business.Models;
using Cancun.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cancun.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }

        public async Task<Address> GetAddressByHotel(Guid hotelId)
        {
            return await Db.Addresss.AsNoTracking()
                .FirstOrDefaultAsync(f => f.HotelId == hotelId);
        }
    }
}