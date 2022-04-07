using CarRentalApi.Shared.Models.Requests;
using FluentValidation;

namespace CarRentalApi.BusinessLayer.Validators
{
    public class SaveReservationRequestValidator : AbstractValidator<SaveReservationRequest>
    {
        public SaveReservationRequestValidator()
        {
            RuleFor(r => r.IdPerson)
                .NotEmpty()
                .WithMessage("the person isn't valid");

            RuleFor(r => r.IdVehicle)
                .NotEmpty()
                .WithMessage("the vehicle isn't valid");
        }
    }
}