using System.Text.RegularExpressions;
using CarRentalApplication.Shared.Models.Requests;
using FluentValidation;

namespace CarRentalApplication.BusinessLayer.Validations;

public partial class SavePersonRequestValidator : AbstractValidator<SavePersonRequest>
{
    public SavePersonRequestValidator()
    {
        RuleFor(p => p.FirstName)
            .MaximumLength(256)
            .NotEmpty()
            .WithMessage("The first name is required");

        RuleFor(p => p.LastName)
            .MaximumLength(256)
            .NotEmpty()
            .WithMessage("The last name is required");

        RuleFor(p => p.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("you can't add a future date");

        RuleFor(p => p.Gender)
            .MaximumLength(10)
            .NotEmpty()
            .WithMessage("the gender is required");

        RuleFor(p => p.IdentityCardNumber)
            .MaximumLength(100)
            .NotEmpty()
            .WithMessage("the card number is required");

        RuleFor(p => p.City)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("the city is required");

        RuleFor(p => p.Province)
            .MaximumLength(20)
            .NotEmpty()
            .WithMessage("the province is required");

        RuleFor(p => p.CellphoneNumber)
            .MaximumLength(20)
            .NotEmpty()
            .WithMessage("the cellphone number is required");
        //.Must(BeAValidPhoneNumber)
        //.WithMessage("insert a valid phone number");

        RuleFor(p => p.EmailAddress)
            .MaximumLength(100)
            .NotEmpty()
            .WithMessage("the email address is required");
    }

    private bool BeAValidPhoneNumber(string cellphoneNumber)
        => CheckPhoneNumber().IsMatch(cellphoneNumber);

    [GeneratedRegex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
    private partial Regex CheckPhoneNumber();
}