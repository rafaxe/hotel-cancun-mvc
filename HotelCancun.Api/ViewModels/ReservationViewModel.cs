using HotelCancun.Api.Extensions;
using HotelCancun.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelCancun.Api.ViewModels
{
    public class CreateReservationViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("Suite")]
        public Guid SuiteId { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("CheckIn")]
        public DateTime CheckIn { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("CheckOut")]
        public DateTime CheckOut { get; set; }
    }

    public class ReservationViewModel : CreateReservationViewModel, IValidatableObject
    {

        public Suite Suite { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<SuiteViewModel> Suites { get; set; }

        [DisplayName("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        [Currency]
        [DisplayName("Price")]
        public decimal PriceTotal { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckIn.Date < DateTime.Now.Date || CheckOut.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult("It is not possible to use a past date");
            }

            if (CheckOut <= CheckIn)
            {
                yield return new ValidationResult("Check Out date should be greater than Check In Date");
            }
        }
    }
}