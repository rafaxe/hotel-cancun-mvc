using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cancun.App.ViewModels
{
    public class HotelViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(14, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 11)]
        public string Document { get; set; }
        public AddressViewModel Address { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        public IEnumerable<SuiteViewModel> Suites { get; set; }
    }
}