using FluentValidation;

namespace Cancun.Business.Models.Validations
{
    public class HotelValidation : AbstractValidator<Hotel>
    {
        public HotelValidation()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("The {PropertyName} field needs to be provided")
                .Length(2, 100)
                .WithMessage("The {PropertyName} field must be between {MinLength} and {MaxLength} characters");
    
        }
    }
}