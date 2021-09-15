using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelCancun.Api.ViewModels
{
    public class BaseAddressViewModel
    {
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
        //Commented validation to facilitate testing
        //[StringLength(8, ErrorMessage = "The field {0} must have {1} characters", MinimumLength = 8)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(50, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Province { get; set; }
    }

    public class EditAddressViewModel : BaseAddressViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid HotelId { get; set; }
    }

    public class AddressViewModel : BaseAddressViewModel
    {
        //Empty constructor to EF Core
        public AddressViewModel()
        {
        }

        public AddressViewModel(BaseAddressViewModel baseViewModel)
        {
            this.City = baseViewModel.City;
            this.Complement = baseViewModel.Complement;
            this.Neighborhood = baseViewModel.Neighborhood;
            this.Number = baseViewModel.Number;
            this.Province = baseViewModel.Province;
            this.Street = baseViewModel.Street;
            this.ZipCode = baseViewModel.ZipCode;
        }

        [Key]
        public Guid Id { get; set; }

        [HiddenInput]
        public Guid HotelId { get; set; }
    }
}