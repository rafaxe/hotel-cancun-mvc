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
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelCancun.Api.Controllers
{

    [Route("reservations")]
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
            var claims = ((ClaimsIdentity)User.Identity);

            if (claims == null)
                return NotFound();

            var claimEmail = claims.FindFirst(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(claimEmail?.Value);

            IEnumerable<ReservationViewModel> reservations;
            if (claims.FindAll(ClaimTypes.Role).FirstOrDefault(x => x.Value == "Manager") != null)
                reservations = _mapper.Map<IEnumerable<ReservationViewModel>>(await _reservationRepository.GetReservationsSuites());
            else
                reservations = _mapper.Map<IEnumerable<ReservationViewModel>>(await _reservationRepository.GetReservationByUserId(user.Id));

            var outputList = reservations.Select(
                x => new OutputReservationViewModel()
                {
                    Id = x.Id,
                    ApplicationUserId = x.ApplicationUserId,
                    CheckIn = x.CheckIn,
                    CheckOut = x.CheckOut,
                    PriceTotal = x.PriceTotal,
                    SuiteId = x.SuiteId,
                }).ToList();

            return Ok(outputList);
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
        public async Task<IActionResult> Create(CreateReservationViewModel createReservationViewModel)
        {
            var reservationViewModel = new ReservationViewModel()
            {
                CheckIn = createReservationViewModel.CheckIn.Date,
                CheckOut = createReservationViewModel.CheckOut.Date,
                Suite = await _suiteRepository.GetById(createReservationViewModel.SuiteId),
                SuiteId = createReservationViewModel.SuiteId,
            };

            await GetUser(reservationViewModel);
            reservationViewModel.ApplicationUserId = reservationViewModel.ApplicationUser.Id;
            if (!ModelState.IsValid) return BadRequest(reservationViewModel);
            await _reservationService.Add(_mapper.Map<Reservation>(reservationViewModel));

            if (!ValidOperation()) return BadRequest(reservationViewModel);

            return Ok(createReservationViewModel);
        }

        [Route("{id:guid}")]
        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, CreateReservationViewModel createReservationViewModel)
        {
            if (id != createReservationViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return BadRequest(createReservationViewModel);
            var reservationUpdate = await GetReservation(id);

            if (reservationUpdate == null)
                return NotFound();

            if (!await AuthorizedUser(reservationUpdate.ApplicationUserId))
            {
                return Unauthorized();
            }

            var reservationViewModel = new ReservationViewModel()
            {
                Id = reservationUpdate.Id,
                ApplicationUser = reservationUpdate.ApplicationUser,
                ApplicationUserId = reservationUpdate.ApplicationUserId,
                CheckIn = createReservationViewModel.CheckIn.Date,
                CheckOut = createReservationViewModel.CheckOut.Date,
                SuiteId = createReservationViewModel.SuiteId,
            };

            reservationViewModel.Suite = await _suiteRepository.GetSuiteHotel(reservationViewModel.SuiteId);
            await _reservationService.Update(_mapper.Map<Reservation>(reservationViewModel));

            if (!ValidOperation()) return BadRequest(reservationViewModel);

            return Ok(createReservationViewModel);
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
        private async Task GetUser(ReservationViewModel reservationViewModel)
        {
            var user = await GetUserApp();
            reservationViewModel.ApplicationUser = user;
            reservationViewModel.ApplicationUserId = user.Id;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> AuthorizedUser(string userId)
        {
            var user = await GetUserApp();

            return user.Id == userId;
        }

        private async Task<ApplicationUser> GetUserApp()
        {
            var claims = ((ClaimsIdentity)User.Identity);

            if (claims == null)
                return null;

            var claimEmail = claims.FindFirst(ClaimTypes.Email);
            return await _userManager.FindByEmailAsync(claimEmail?.Value);
        }
    }
}
