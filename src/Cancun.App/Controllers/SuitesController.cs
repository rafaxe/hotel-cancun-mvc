using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Cancun.App.Extensions;
using Microsoft.AspNetCore.Mvc;
using Cancun.App.ViewModels;
using Cancun.Business.Intefaces;
using Cancun.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Cancun.App.Controllers
{
    [Authorize]
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [Route("list-suites")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<SuiteViewModel>>(await _suiteRepository.GetSuitesHotels()));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [Route("data-suite/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var suiteViewModel = await GetSuite(id);

            if (suiteViewModel == null)
            {
                return NotFound();
            }

            return View(suiteViewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Suite","Add")]
        [Route("new-suite")]
        public async Task<IActionResult> Create()
        {
            var suiteViewModel = await PopulateHotels(new SuiteViewModel());

            return View(suiteViewModel);
        }

        [ClaimsAuthorize("Suite", "Add")]
        [Route("new-suite")]
        [HttpPost]
        public async Task<IActionResult> Create(SuiteViewModel suiteViewModel)
        {
            suiteViewModel = await PopulateHotels(suiteViewModel);
            if (!ModelState.IsValid) return View(suiteViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(suiteViewModel.ImageUpload, imgPrefixo))
            {
                return View(suiteViewModel);
            }

            suiteViewModel.Image = imgPrefixo + suiteViewModel.ImageUpload.FileName;
            await _suiteService.Add(_mapper.Map<Suite>(suiteViewModel));

            if (!ValidOperation()) return View(suiteViewModel);

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Suite", "Edit")]
        [Route("edit-suite/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var suiteViewModel = await GetSuite(id);

            if (suiteViewModel == null)
            {
                return NotFound();
            }

            return View(suiteViewModel);
        }

        [ClaimsAuthorize("Suite", "Edit")]
        [Route("edit-suite/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, SuiteViewModel suiteViewModel)
        {
            if (id != suiteViewModel.Id) return NotFound();

            var suiteUpdate = await GetSuite(id);
            suiteViewModel.Hotel = suiteUpdate.Hotel;
            suiteViewModel.Image = suiteUpdate.Image;
            if (!ModelState.IsValid) return View(suiteViewModel);

            if (suiteViewModel.ImageUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(suiteViewModel.ImageUpload, imgPrefixo))
                {
                    return View(suiteViewModel);
                }

                suiteUpdate.Image = imgPrefixo + suiteViewModel.ImageUpload.FileName;
            }

            suiteUpdate.Name = suiteViewModel.Name;
            suiteUpdate.Description = suiteViewModel.Description;
            suiteUpdate.Price = suiteViewModel.Price;
            suiteUpdate.Active = suiteViewModel.Active;

            await _suiteService.Update(_mapper.Map<Suite>(suiteUpdate));

            if (!ValidOperation()) return View(suiteViewModel);

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [ClaimsAuthorize("Suite", "Remove")]
        [Route("remove-suite/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var suite = await GetSuite(id);

            if (suite == null)
            {
                return NotFound();
            }

            return View(suite);
        }

        [ClaimsAuthorize("Suite", "Remove")]
        [Route("remove-suite/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var suite = await GetSuite(id);

            if (suite == null)
            {
                return NotFound();
            }

            await _suiteService.Remove(id);

            if (!ValidOperation()) return View(suite);

            TempData["Sucesso"] = "Suite successfully deleted!";

            return RedirectToAction("Index");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<SuiteViewModel> GetSuite(Guid id)
        {
            var suite = _mapper.Map<SuiteViewModel>(await _suiteRepository.GetSuiteHotel(id));
            suite.Hotels = _mapper.Map<IEnumerable<HotelViewModel>>(await _hotelRepository.GetAll());
            return suite;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<SuiteViewModel> PopulateHotels(SuiteViewModel suite)
        {
            suite.Hotels = _mapper.Map<IEnumerable<HotelViewModel>>(await _hotelRepository.GetAll());
            return suite;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "A file with this name already exists");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
