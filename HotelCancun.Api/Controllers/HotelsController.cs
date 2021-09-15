using AutoMapper;
using HotelCancun.Api.ViewModels;
using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelCancun.Api.Controllers
{
    [Authorize]
    [Route("hotel")]
    public class HotelsController : BaseController
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelService _hotelService;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository hotelRepository,
              IMapper mapper,
              IHotelService hotelService,
              INotifier notifier) : base(notifier)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _hotelService = hotelService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(_mapper.Map<IEnumerable<HotelViewModel>>(await _hotelRepository.GetAll()));
        }

        [AllowAnonymous]
        [Route("{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var hotelViewModel = await GetHotelAddress(id);

            if (hotelViewModel == null)
            {
                return NotFound();
            }

            return Ok(hotelViewModel);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateHotelViewModel createHotelViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(createHotelViewModel);

            var hotelViewModel = new HotelViewModel()
            {
                Active = createHotelViewModel.Active,
                Address = new AddressViewModel(createHotelViewModel.Address),
                Document = createHotelViewModel.Document,
                Name = createHotelViewModel.Name,
            };

            var hotel = _mapper.Map<Hotel>(hotelViewModel);
            await _hotelService.Add(hotel);

            if (!ValidOperation()) return BadRequest(hotelViewModel);

            return Ok(hotelViewModel);
        }

        [Authorize(Roles = "Manager")]
        [Route("{id:guid}")]
        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, EditHotelViewModel editHotelViewModel)
        {
            if (id != editHotelViewModel.Id) return NotFound();

            var hotelViewModel = new HotelViewModel()
            {
                Id = editHotelViewModel.Id,
                Active = editHotelViewModel.Active,
                Document = editHotelViewModel.Document,
                Name = editHotelViewModel.Name,
            };

            if (!ModelState.IsValid) return BadRequest(hotelViewModel);

            var hotel = _mapper.Map<Hotel>(hotelViewModel);

            await _hotelService.Update(hotel);

            if (!ValidOperation()) return BadRequest();

            return Ok(hotelViewModel);
        }

        [Authorize(Roles = "Manager")]
        [Route("address/{id:guid}")]
        [HttpPut]
        public async Task<IActionResult> EditAddress(Guid id, EditAddressViewModel editAddressViewModel)
        {
            if (id != editAddressViewModel.Id) return NotFound();

            var addressViewModel = new AddressViewModel(editAddressViewModel)
            {
                Id = editAddressViewModel.Id
            };

            if (!ModelState.IsValid) return BadRequest(addressViewModel);

            var address = _mapper.Map<Address>(addressViewModel);
            await _hotelService.UpdateAddress(address);

            if (!ValidOperation()) return BadRequest();

            return Ok(addressViewModel);
        }

        [Authorize(Roles = "Manager")]
        [Route("{id:guid}")]
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var hotel = await GetHotelAddress(id);

            if (hotel == null) return NotFound();

            await _hotelService.Remove(id);

            if (!ValidOperation()) return BadRequest(hotel);

            return Ok("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<HotelViewModel> GetHotelAddress(Guid id)
        {
            return _mapper.Map<HotelViewModel>(await _hotelRepository.GetHotelAddress(id));
        }
    }
}
