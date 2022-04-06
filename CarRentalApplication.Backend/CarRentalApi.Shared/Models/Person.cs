using CarRentalApi.Shared.Models.Common;

namespace CarRentalApi.Shared.Models;

public class Person : BaseObject
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public string PhoneNumber { get; set; }

    public string EmailAddress { get; set; }
}