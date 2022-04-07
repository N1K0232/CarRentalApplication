using CarRentalApi.Shared.Models.Requests;
using FluentValidation;

namespace CarRentalApi.BusinessLayer.Validators
{
    public class SavePersonRequestValidator : AbstractValidator<SavePersonRequest>
    {
        public SavePersonRequestValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't add a person without the name");

            RuleFor(p => p.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't add a person without the surname");

            RuleFor(p => p.BirthDate)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't add a person without the birth date");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't add a person without the phone number");

            RuleFor(p => p.EmailAddress)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't add a person without the email address");
        }
    }
}