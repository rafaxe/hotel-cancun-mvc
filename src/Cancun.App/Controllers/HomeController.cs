using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cancun.App.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cancun.App.Controllers
{
    public class HomeController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Message = "An error has occurred! Please try again later or contact our support.";
                modelErro.Title = "Error!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Message = "The page you are looking for does not exist! <br />If you have any questions, please contact our support";
                modelErro.Title = "Oops! Page not found.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Message = "You are not allowed to do this.";
                modelErro.Title = "Access denied";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}
