using CarRentalApi.Shared.Models.Requests;
using FluentValidation;

namespace CarRentalApi.BusinessLayer.Validators
{
    public class SaveVehicleRequestValidator : AbstractValidator<SaveVehicleRequest>
    {
        public SaveVehicleRequestValidator()
        {
            RuleFor(v => v.Brand)
                .NotNull()
                .NotEmpty()
                .WithMessage("can't register a vehicle with no brand");

            RuleFor(v => v.Model)
                .NotNull()
                .NotEmpty()
                .WithMessage("can't register a vehicle with no model");

            RuleFor(v => v.Plate)
                .NotNull()
                .NotEmpty()
                .WithMessage("can't register a vehicle with no plate");
        }
    }
}