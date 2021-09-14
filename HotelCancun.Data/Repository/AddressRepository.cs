using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HotelCancun.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context) { }

        public async Task<Address> GetAddressByHotel(Guid hotelId)
        {
            return await Db.Address.AsNoTracking()
                .FirstOrDefaultAsync(f => f.HotelId == hotelId);
        }
    }
}