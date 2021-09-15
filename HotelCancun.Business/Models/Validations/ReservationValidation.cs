using FluentValidation;

namespace HotelCancun.Business.Models.Validations
{
    public class ReservationValidation : AbstractValidator<Reservation>
    {
        public ReservationValidation()
        {
            RuleFor(c => c.Days)
                .GreaterThanOrEqualTo(1).WithMessage("The {PropertyName} field must be greater than or equal to {ComparisonValue}")
                .LessThanOrEqualTo(3).WithMessage("The {PropertyName} field must be less than or equal to {ComparisonValue}");

        }
    }
}