using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using HotelCancun.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelCancun.Business.Services
{
    public class HotelService : BaseService, IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IAddressRepository _addressRepository;

        public HotelService(IHotelRepository hotelRepository, 
                                 IAddressRepository addressRepository,
                                 INotifier notifier) : base(notifier)
        {
            _hotelRepository = hotelRepository;
            _addressRepository = addressRepository;
        }

        public async Task Add(Hotel hotel)
        {
            if (!ExecuteValidation(new HotelValidation(), hotel) 
                || !ExecuteValidation(new AddressValidation(), hotel.Address)) return;

            if (_hotelRepository.Search(f => f.Document == hotel.Document).Result.Any())
            {
                Notify("There is already a hotel with this document informed.");
                return;
            }

            await _hotelRepository.Add(hotel);
        }

        public async Task Update(Hotel hotel)
        {
            if (!ExecuteValidation(new HotelValidation(), hotel)) return;
            await _hotelRepository.Update(hotel);
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address)) return;

            await _addressRepository.Update(address);
        }

        public async Task Remove(Guid id)
        {
            if (_hotelRepository.GetHotelSuitesAddress(id).Result.Suites.Any())
            {
                Notify("The hotel has registered suites");
                return;
            }

            var address = await _addressRepository.GetAddressByHotel(id);

            if (address != null)
            {
                await _addressRepository.Remove(address.Id);
            }

            await _hotelRepository.Remove(id);
        }

        public void Dispose()
        {
            _hotelRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}