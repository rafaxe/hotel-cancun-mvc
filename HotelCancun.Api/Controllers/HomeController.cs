using HotelCancun.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelCancun.Api.Controllers
{
    public class HomeController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("error/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelError = new ErrorViewModel();

            switch (id)
            {
                case 500:
                    modelError.Message = "An error has occurred! Please try again later or contact our support.";
                    modelError.Title = "Error!";
                    modelError.ErrorCode = id;
                    break;
                case 404:
                    modelError.Message = "The page you are looking for does not exist! <br />If you have any questions, please contact our support";
                    modelError.Title = "Oops! Page not found.";
                    modelError.ErrorCode = id;
                    break;
                case 403:
                    modelError.Message = "You are not allowed to do this.";
                    modelError.Title = "Access denied";
                    modelError.ErrorCode = id;
                    break;
                default:
                    return StatusCode(500);
            }

            return BadRequest(modelError);
        }
    }
}
