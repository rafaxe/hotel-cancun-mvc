//using AutoMapper;
//using HotelCancun.Api.ViewModels;
//using HotelCancun.Business.Interfaces;
//using HotelCancun.Business.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using HotelCancun.Api.Extensions;

//namespace HotelCancun.Api.Controllers
//{
//    [Authorize]
//    [Route("suite")]
//    public class SuitesController : BaseController
//    {
//        private readonly ISuiteRepository _suiteRepository;
//        private readonly IHotelRepository _hotelRepository;
//        private readonly ISuiteService _suiteService;
//        private readonly IMapper _mapper;

//        public SuitesController(ISuiteRepository suiteRepository,
//                                  IHotelRepository hotelRepository,
//                                  IMapper mapper,
//                                  ISuiteService suiteService,
//                                  INotifier notifier) : base(notifier)
//        {
//            _suiteRepository = suiteRepository;
//            _hotelRepository = hotelRepository;
//            _mapper = mapper;
//            _suiteService = suiteService;
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            return Ok(_mapper.Map<IEnumerable<SuiteViewModel>>(await _suiteRepository.GetSuitesHotels()));
//        }

//        [AllowAnonymous]
//        [Route("details/{id:guid}")]
//        [HttpGet]
//        public async Task<IActionResult> Details(Guid id)
//        {
//            var suiteViewModel = await GetSuite(id);

//            if (suiteViewModel == null)
//            {
//                return NotFound();
//            }

//            return Ok(suiteViewModel);
//        }

//        [ClaimsAuthorize("Suite", "Add")]
//        [HttpPost]
//        public async Task<IActionResult> Create(SuiteViewModel suiteViewModel)
//        {
//            suiteViewModel = await PopulateHotels(suiteViewModel);
//            if (!ModelState.IsValid) return BadRequest(suiteViewModel);

//            var imgPrefix = Guid.NewGuid() + "_";
//            await UploadFile(suiteViewModel.ImageUpload, imgPrefix);

//            suiteViewModel.Image = imgPrefix + suiteViewModel.ImageUpload.FileName;
//            await _suiteService.Add(_mapper.Map<Suite>(suiteViewModel));

//            if (!ValidOperation()) return BadRequest(suiteViewModel);

//            return Ok(suiteViewModel);
//        }

//        [ClaimsAuthorize("Suite", "Edit")]
//        [Route("{id:guid}")]
//        [HttpPut]
//        public async Task<IActionResult> Edit(Guid id, SuiteViewModel suiteViewModel)
//        {
//            if (id != suiteViewModel.Id) return NotFound();

//            var suiteUpdate = await GetSuite(id);
//            suiteViewModel.Hotel = suiteUpdate.Hotel;
//            suiteViewModel.Image = suiteUpdate.Image;
//            if (!ModelState.IsValid) return BadRequest(suiteViewModel);

//            if (suiteViewModel.ImageUpload != null)
//            {
//                var imgPrefix = Guid.NewGuid() + "_";
//                await UploadFile(suiteViewModel.ImageUpload, imgPrefix);
//                suiteUpdate.Image = imgPrefix + suiteViewModel.ImageUpload.FileName;
//            }

//            suiteUpdate.Name = suiteViewModel.Name;
//            suiteUpdate.Description = suiteViewModel.Description;
//            suiteUpdate.Price = suiteViewModel.Price;
//            suiteUpdate.Active = suiteViewModel.Active;

//            await _suiteService.Update(_mapper.Map<Suite>(suiteUpdate));

//            if (!ValidOperation()) return BadRequest(suiteViewModel);

//            return Ok(suiteViewModel);
//        }

//        [ClaimsAuthorize("Suite", "Remove")]
//        [HttpDelete, ActionName("Delete")]
//        public async Task<IActionResult> DeleteConfirmed(Guid id)
//        {
//            var suite = await GetSuite(id);

//            if (suite == null)
//            {
//                return NotFound();
//            }

//            await _suiteService.Remove(id);

//            if (!ValidOperation()) return BadRequest(suite);

//            return Ok();
//        }

//        [ApiExplorerSettings(IgnoreApi = true)]
//        private async Task<SuiteViewModel> GetSuite(Guid id)
//        {
//            var suite = _mapper.Map<SuiteViewModel>(await _suiteRepository.GetSuiteHotel(id));
//            suite.Hotels = _mapper.Map<IEnumerable<HotelViewModel>>(await _hotelRepository.GetAll());
//            return suite;
//        }

//        [ApiExplorerSettings(IgnoreApi = true)]
//        private async Task<SuiteViewModel> PopulateHotels(SuiteViewModel suite)
//        {
//            suite.Hotels = _mapper.Map<IEnumerable<HotelViewModel>>(await _hotelRepository.GetAll());
//            return suite;
//        }

//        [ApiExplorerSettings(IgnoreApi = true)]
//        private async Task UploadFile(IFormFile file, string imgPrefix)
//        {
//            if (file.Length <= 0) return;

//            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefix + file.FileName);

//            if (System.IO.File.Exists(path))
//            {
//                ModelState.AddModelError(string.Empty, "A file with this name already exists");
//                return;
//            }

//            await using var stream = new FileStream(path, FileMode.Create);
//            await file.CopyToAsync(stream);
//        }
//    }
//}
