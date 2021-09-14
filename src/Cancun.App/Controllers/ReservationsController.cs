using AutoMapper;
using Cancun.App.Extensions;
using Cancun.App.ViewModels;
using Cancun.Business.Intefaces;
using Cancun.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;

namespace Cancun.App.Controllers
{
    [Authorize]
    public class ReservationsController : BaseController
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISuiteRepository _suiteRepository;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationsController(IReservationRepository reservationRepository,
                                  ISuiteRepository suiteRepository,
                                  IMapper mapper,
                                  IReservationService reservationService,
                                  INotifier notifier,
                                  UserManager<IdentityUser> userManager) : base(notifier)
        {
            _reservationRepository = reservationRepository;
            _suiteRepository = suiteRepository;
            _mapper = mapper;
            _reservationService = reservationService;
            _userManager = userManager;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [Route("list-reservations")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user);

            if (claims.FirstOrDefault(x => x.Type == "Hotel") != null)
                return View(_mapper.Map<IEnumerable<ReservationViewModel>>(await _reservationRepository.GetReservationsSuites()));
            else
                return View(_mapper.Map<IEnumerable<ReservationViewModel>>(await _reservationRepository.GetReservationByUserId(user.Id)));

        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [Route("data-reservation/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var reservationViewModel = await GetReservation(id);

            if (!await AuthorizedUser(reservationViewModel.ApplicationUserId))
            {
                return Unauthorized();
            }

            return View(reservationViewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [Route("new-reservation")]
        public async Task<IActionResult> Create()
        {
            var reservationViewModel = await PopulateSuites(new ReservationViewModel());

            return View(reservationViewModel);
        }

        [Authorize]
        [Route("new-reservation")]
        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel reservationViewModel)
        {
            reservationViewModel = await PopulateSuites(reservationViewModel);
            await GetUser(reservationViewModel);

            if (!await AuthorizedUser(reservationViewModel.ApplicationUserId))
            {
                return Unauthorized();
            }

            reservationViewModel.Suite = await _suiteRepository.GetById(reservationViewModel.SuiteId);
            reservationViewModel.CheckIn = reservationViewModel.CheckIn.Date;
            reservationViewModel.CheckOut = reservationViewModel.CheckOut.Date;
            reservationViewModel.RecalculatePrice();

            if (!ModelState.IsValid) return View(reservationViewModel);
            await _reservationService.Add(_mapper.Map<Reservation>(reservationViewModel));

            if (!ValidOperation()) return View(reservationViewModel);

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [Route("edit-reservation/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var reservationViewModel = await GetReservation(id);
            reservationViewModel = await PopulateSuites(reservationViewModel);

            if (reservationViewModel == null)
            {
                return NotFound();
            }

            if (!await AuthorizedUser(reservationViewModel.ApplicationUserId))
            {
                return Unauthorized();
            }

            return View(reservationViewModel);
        }

        [Authorize]
        [Route("edit-reservation/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ReservationViewModel reservationViewModel)
        {
            if (id != reservationViewModel.Id) return NotFound();
            reservationViewModel = await PopulateSuites(reservationViewModel);

            if (!await AuthorizedUser(reservationViewModel.ApplicationUserId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid) return View(reservationViewModel);
            var reservationUpdate = await GetReservation(id);

            reservationViewModel.Suite = reservationUpdate.Suite;
            reservationUpdate.ApplicationUser = reservationViewModel.ApplicationUser;
            reservationUpdate.CheckIn = reservationViewModel.CheckIn.Date;
            reservationUpdate.CheckOut = reservationViewModel.CheckOut.Date;
            reservationUpdate.RecalculatePrice();

            await _reservationService.Update(_mapper.Map<Reservation>(reservationUpdate));

            if (!ValidOperation()) return View(reservationViewModel);

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        [Route("remove-reservation/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
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

            return View(reservationViewModel);
        }

        [Authorize]
        [Route("remove-reservation/{id:guid}")]
        [HttpPost, ActionName("Delete")]
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

            if (!ValidOperation()) return View(reservationViewModel);

            TempData["Sucesso"] = "Reservation successfully deleted!";

            return RedirectToAction("Index");
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
        private async Task<ReservationViewModel> GetUser(ReservationViewModel reservationViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            reservationViewModel.ApplicationUser = user;
            reservationViewModel.ApplicationUserId = user.Id;

            return reservationViewModel;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> AuthorizedUser(string userId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.Id != userId)
            {
                return false;
            }
            return true;
        }
    }
}
