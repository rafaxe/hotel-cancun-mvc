using AutoMapper;
using HotelCancun.Api.Configurations;
using HotelCancun.Api.ViewModels;
using HotelCancun.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelCancun.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserViewModel model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) return Ok();

            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] UserViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return NotFound(new { message = "username or password is invalid" });

            var token = TokenConfiguration.GenerateToken(user);

            user.PasswordHash = "";

            return new
            {
                user.Email,
                token
            };
        }
    }
}