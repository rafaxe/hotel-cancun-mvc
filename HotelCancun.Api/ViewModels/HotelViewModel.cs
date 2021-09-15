﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelCancun.Api.ViewModels
{
    public class BaseHotelViewModel
    {
        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(14, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 11)]
        public string Document { get; set; }
        public BaseAddressViewModel Address { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }
    }

    public class EditHotelViewModel : BaseHotelViewModel
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class HotelViewModel : BaseHotelViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public new AddressViewModel Address { get; set; }

        public IEnumerable<SuiteViewModel> Suites { get; set; }
    }

    public class AddressHotelViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public new AddressViewModel Address { get; set; }
    }
}