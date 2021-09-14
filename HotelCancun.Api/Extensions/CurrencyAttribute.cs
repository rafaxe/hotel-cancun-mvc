using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HotelCancun.Api.Extensions
{
    public class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var currency = Convert.ToDecimal(value, new CultureInfo("en-CA"));
            }           
            catch (Exception)     
            {
                return new ValidationResult("Invalid format currency");
            }

            return ValidationResult.Success;
        }
    }
}