using AutoMapper;
using HotelCancun.Api.ViewModels;
using HotelCancun.Business.Interfaces;
using HotelCancun.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HotelCancun.Api.Controllers
{
    [Authorize]
    [Route("suites")]
    public class SuitesController : BaseController
    {
        private readonly ISuiteRepository _suiteRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ISuiteService _suiteService;
        private readonly IMapper _mapper;

        public SuitesController(ISuiteRepository suiteRepository,
                                  IHotelRepository hotelRepository,
                                  IMapper mapper,
                                  ISuiteService suiteService,
                                  INotifier notifier) : base(notifier)
        {
            _suiteRepository = suiteRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _suiteService = suiteService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(_mapper.Map<IEnumerable<SuiteViewModel>>(await _suiteRepository.GetSuites()));
        }

        [AllowAnonymous]
        [Route("{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var suiteViewModel = await GetSuite(id);

            if (suiteViewModel == null)
            {
                return NotFound();
            }

            return Ok(suiteViewModel);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(BaseSuiteViewModel baseSuiteViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(baseSuiteViewModel);

            var imgPrefix = Guid.NewGuid() + "_";
            await UploadFile(baseSuiteViewModel.ImageUpload, imgPrefix);

            var suiteViewModel = new SuiteViewModel
            {
                Name = baseSuiteViewModel.Name,
                Active = baseSuiteViewModel.Active,
                HotelId = baseSuiteViewModel.HotelId,
                Description = baseSuiteViewModel.Description,
                ImageUpload = baseSuiteViewModel.ImageUpload,
                Price = baseSuiteViewModel.Price,
                RegistrationDate = baseSuiteViewModel.RegistrationDate,
                Image = baseSuiteViewModel.ImageUpload != null? imgPrefix + baseSuiteViewModel.ImageUpload.FileName: string.Empty,
            };


            await _suiteService.Add(_mapper.Map<Suite>(suiteViewModel));

            if (!ValidOperation()) return BadRequest(GetNotifications());

            return Ok(suiteViewModel);
        }

        [Authorize(Roles = "Manager")]
        [Route("{id:guid}")]
        [HttpPut]
        public async Task<IActionResult> Edit(Guid id, EditSuiteViewModel editSuiteViewModel)
        {
            if (id != editSuiteViewModel.Id) return NotFound();

            var suiteViewModel = new SuiteViewModel(editSuiteViewModel);
            var suiteUpdate = await GetSuite(id);

            await _hotelRepository.GetById(suiteViewModel.HotelId);
            var hotel = await _hotelRepository.GetById(suiteViewModel.HotelId);
            var hotelViewModel = _mapper.Map<HotelViewModel>(hotel);

            suiteViewModel.Hotel = hotelViewModel;
            suiteViewModel.Image = suiteUpdate.Image;
            if (!ModelState.IsValid) return BadRequest(suiteViewModel);

            if (suiteViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";
                await UploadFile(suiteViewModel.ImageUpload, imgPrefix);
                suiteUpdate.Image = imgPrefix + suiteViewModel.ImageUpload.FileName;
            }

            suiteUpdate.Name = suiteViewModel.Name;
            suiteUpdate.Description = suiteViewModel.Description;
            suiteUpdate.Price = suiteViewModel.Price;
            suiteUpdate.Active = suiteViewModel.Active;
            suiteUpdate.Hotel = suiteViewModel.Hotel;

            await _suiteService.Update(_mapper.Map<Suite>(suiteUpdate));

            if (!ValidOperation()) return BadRequest(GetNotifications());

            return Ok(suiteViewModel);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var suite = await GetSuite(id);

            if (suite == null)
            {
                return NotFound();
            }

            await _suiteService.Remove(id);

            if (!ValidOperation()) return BadRequest(GetNotifications());

            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<SuiteViewModel> GetSuite(Guid id)
        {
            var suite = _mapper.Map<SuiteViewModel>(await _suiteRepository.GetSuiteHotel(id));
            
            if(suite != null) suite.Hotel = null;
            return suite;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task UploadFile(IFormFile file, string imgPrefix)
        {
            if(file == null) return;

            if (file.Length <= 0) return;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imgPrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "A file with this name already exists");
                return;
            }

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}
