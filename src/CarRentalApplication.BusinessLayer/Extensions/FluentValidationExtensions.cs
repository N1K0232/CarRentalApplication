using FluentValidation.Results;
using OperationResults;

namespace CarRentalApplication.BusinessLayer.Extensions;

public static class FluentValidationExtensions
{
    public static IEnumerable<ValidationError> ToValidationErrors(this IEnumerable<ValidationFailure> errors)
    {
        var validationErrors = new List<ValidationError>();
        foreach (var error in errors)
        {
            validationErrors.Add(new ValidationError(error.PropertyName, error.ErrorMessage));
        }

        return validationErrors;
    }
}