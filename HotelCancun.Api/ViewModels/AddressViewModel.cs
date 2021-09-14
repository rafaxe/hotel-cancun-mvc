using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HotelCancun.Api.ViewModels
{
    public class AddressViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(200, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Street { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(50, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 1)]
        public string Number { get; set; }

        public string Complement { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Neighborhood { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(8, ErrorMessage = "The field {0} must have {1} characters", MinimumLength = 8)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(50, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Province { get; set; }

        [HiddenInput]
        public Guid HotelId { get; set; }
    }
}