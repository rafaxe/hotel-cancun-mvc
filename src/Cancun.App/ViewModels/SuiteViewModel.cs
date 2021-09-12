using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Cancun.App.Extensions;
using Cancun.Business.Models;
using Microsoft.AspNetCore.Http;

namespace Cancun.App.ViewModels
{
    public class SuiteViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [DisplayName("Hotel")]
        public Guid HotelId { get; set; }

        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(200, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "{0} is a Required field")]
        [StringLength(1000, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        [DisplayName("Image")]
        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Currency]
        [Required(ErrorMessage = "{0} is a Required field")]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime RegistrationDate { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        public HotelViewModel Hotel { get; set; }

        public IEnumerable<HotelViewModel> Hotels { get; set; }
    }
}