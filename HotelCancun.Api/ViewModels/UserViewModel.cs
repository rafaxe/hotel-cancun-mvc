using System.ComponentModel.DataAnnotations;

namespace HotelCancun.Api.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "{0} is a Required field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        public string Password { get; set; }
    }
}   