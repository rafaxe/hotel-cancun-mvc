using System;
using System.Threading.Tasks;
using Cancun.Business.Models;

namespace Cancun.Business.Intefaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressByHotel(Guid hotelId);
    }
}