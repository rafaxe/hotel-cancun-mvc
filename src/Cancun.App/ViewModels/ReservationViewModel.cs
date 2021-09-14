using Cancun.App.Extensions;
using Cancun.Business.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cancun.App.ViewModels
{
    public class ReservationViewModel : IValidatableObject
    {
        [Key]
        public Guid Id { get; set; }
        public Suite Suite { get; set; }
        public IdentityUser ApplicationUser { get; set; }
        public IEnumerable<SuiteViewModel> Suites { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("Suite")]
        public Guid SuiteId { get; set; }


        [DisplayName("User")]
        public string ApplicationUserId { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("CheckIn")]
        public DateTime CheckIn { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("CheckOut")]
        public DateTime CheckOut { get; set; }

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