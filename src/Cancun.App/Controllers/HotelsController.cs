using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cancun.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using Cancun.App.ViewModels;
using Cancun.Business.Intefaces;
using Cancun.Business.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cancun.App.Controllers
{
    [Authorize]
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [Route("list-hotels")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<HotelViewModel>>(await _hotelRepository.GetAll()));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [Route("data-hotel/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var hotelViewModel = await GetHotelAddress(id);

            if (hotelViewModel == null)
            {
                return NotFound();
            }

            return View(hotelViewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Hotel", "Add")]
        [Route("new-hotel")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Hotel", "Add")]
        [Route("new-hotel")]
        [HttpPost]
        public async Task<IActionResult> Create(HotelViewModel hotelViewModel)
        {
            if (!ModelState.IsValid) return View(hotelViewModel);

            var hotel = _mapper.Map<Hotel>(hotelViewModel);
            await _hotelService.Add(hotel);

            if (!ValidOperation()) return View(hotelViewModel);

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Hotel", "Edit")]
        [Route("edit-hotel/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var hotelViewModel = await GetHotelSuitesAddress(id);

            if (hotelViewModel == null)
            {
                return NotFound();
            }

            return View(hotelViewModel);
        }

        [ClaimsAuthorize("Hotel", "Edit")]
        [Route("edit-hotel/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, HotelViewModel hotelViewModel)
        {
            if (id != hotelViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(hotelViewModel);

            var hotel = _mapper.Map<Hotel>(hotelViewModel);
            await _hotelService.Update(hotel);

            if (!ValidOperation()) return View(await GetHotelSuitesAddress(id));

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Hotel", "Remove")]
        [Route("remove-hotel/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var hotelViewModel = await GetHotelAddress(id);

            if (hotelViewModel == null)
            {
                return NotFound();
            }

            return View(hotelViewModel);
        }

        [ClaimsAuthorize("Hotel", "Remove")]
        [Route("remove-hotel/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var hotel = await GetHotelAddress(id);

            if (hotel == null) return NotFound();

            await _hotelService.Remove(id);

            if (!ValidOperation()) return View(hotel);

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [Route("get-hotel-address/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var hotel = await GetHotelAddress(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsAddress", hotel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Hotel", "Edit")]
        [Route("update-hotel-address/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id)
        {
            var hotel = await GetHotelAddress(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return PartialView("_UpdateAddress", new HotelViewModel { Address = hotel.Address });
        }

        [ClaimsAuthorize("Hotel", "Edit")]
        [Route("update-hotel-address/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(HotelViewModel hotelViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if (!ModelState.IsValid) return PartialView("_UpdateAddress", hotelViewModel);

            await _hotelService.UpdateAddress(_mapper.Map<Address>(hotelViewModel.Address));

            if (!ValidOperation()) return PartialView("_UpdateAddress", hotelViewModel);

            var url = Url.Action("GetAddress", "Hotels", new { id = hotelViewModel.Address.HotelId });
            return Json(new { success = true, url });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<HotelViewModel> GetHotelAddress(Guid id)
        {
            return _mapper.Map<HotelViewModel>(await _hotelRepository.GetHotelAddress(id));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<HotelViewModel> GetHotelSuitesAddress(Guid id)
        {
            return _mapper.Map<HotelViewModel>(await _hotelRepository.GetHotelSuitesAddress(id));
        }
    }
}
