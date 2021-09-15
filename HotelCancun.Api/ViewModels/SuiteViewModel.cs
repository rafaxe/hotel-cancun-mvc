using HotelCancun.Api.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelCancun.Api.ViewModels
{
    public class BaseSuiteViewModel
    {
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

        [Currency]
        [Required(ErrorMessage = "{0} is a Required field")]
        public decimal Price { get; set; }

        [DisplayName("Active?")]
        public bool Active { get; set; }

        [ScaffoldColumn(false)]
        public DateTime RegistrationDate { get; set; }

    }

    public class EditSuiteViewModel : BaseSuiteViewModel
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class SuiteViewModel : BaseSuiteViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Image { get; set; }

        public HotelViewModel Hotel { get; set; }

        public IEnumerable<HotelViewModel> Hotels { get; set; }
    }
}