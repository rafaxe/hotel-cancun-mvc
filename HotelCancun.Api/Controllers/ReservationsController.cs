using AutoMapper;
using HotelCancun.Api.ViewModels;
using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelCancun.Api.Data;

namespace HotelCancun.Api.Controllers
{

    [Route("reservation")]
    [Authorize]
    public class ReservationsController : BaseController
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISuiteRepository _suiteRepository;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsController(IReservationRepository reservationRepository,
                                  ISuiteRepository suiteRepository,
                                  IMapper mapper,
                                  IReservationService reservationService,
                                  INotifier notifier,
                                  UserManager<ApplicationUser> userManager) : base(notifier)
        {
            _reservationRepository = reservationRepository;
            _suiteRepository = suiteRepository;
            _mapper = mapper;
            _reservationService = reservationService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.FirstOrDefault(x => x.Type == "Hotel") != null)
                return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(await _reservationRepository.GetReservationsSuites()));

            return Ok(_mapper.Map<IEnumerable<ReservationViewModel>>(await _reservationRepository.GetReservationByUserId(user.Id)));

        }

        [Route("{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var reservationViewModel = await GetReservation(id);

            if (!await AuthorizedUser(reservationViewModel.ApplicationUserId))
            {
                return Unauthorized();
            }

            return Ok(reservationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel reservationViewModel)
        {
            reservationViewModel = await PopulateSuites(reservationViewModel);
            await GetUser(reservationViewModel);

            reservationViewModel.Suite = await _suiteRepository.GetById(reservationViewModel.SuiteId);
            reservationViewModel.CheckIn = reservationViewModel.CheckIn.Date;
            reservationViewModel.CheckOut = reservationViewModel.CheckOut.Date;

            if (!ModelState.IsValid) return BadRequest(reservationViewModel);
            await _reservationService.Add(_mapper.Map<Reservation>(reservationViewModel));

            if (!ValidOperation()) return BadRequest(reservationViewModel);

            return Ok(reservationViewModel);
        }

        [Route("{id:guid}")]
        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, ReservationViewModel reservationViewModel)
        {
            if (id != reservationViewModel.Id) return NotFound();
            reservationViewModel = await PopulateSuites(reservationViewModel);

            if (!ModelState.IsValid) return BadRequest(reservationViewModel);
            var reservationUpdate = await GetReservation(id);

            if (!await AuthorizedUser(reservationUpdate.ApplicationUserId))
            {
                return Unauthorized();
            }

            reservationViewModel.Suite = reservationUpdate.Suite;
            reservationUpdate.ApplicationUser = reservationViewModel.ApplicationUser;
            reservationUpdate.CheckIn = reservationViewModel.CheckIn.Date;
            reservationUpdate.CheckOut = reservationViewModel.CheckOut.Date;

            await _reservationService.Update(_mapper.Map<Reservation>(reservationUpdate));

            if (!ValidOperation()) return BadRequest(reservationViewModel);

            return Ok(reservationViewModel);
        }

        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reservationViewModel = await GetReservation(id);

            if (reservationViewModel == null)
            {
                return NotFound();
            }

            if (!await AuthorizedUser(reservationViewModel.ApplicationUserId))
            {
                return Unauthorized();
            }

            await _reservationService.Remove(id);
            if (!ValidOperation()) return BadRequest(reservationViewModel);
            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<ReservationViewModel> GetReservation(Guid id)
        {
            return _mapper.Map<ReservationViewModel>(await _reservationRepository.GetReservation(id));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<ReservationViewModel> PopulateSuites(ReservationViewModel reservation)
        {
            reservation.Suites = _mapper.Map<IEnumerable<SuiteViewModel>>(await _suiteRepository.GetAll());
            return reservation;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task GetUser(ReservationViewModel reservationViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            reservationViewModel.ApplicationUser = user;
            reservationViewModel.ApplicationUserId = user.Id;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> AuthorizedUser(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            return user.Id == userId;
        }
    }
}
