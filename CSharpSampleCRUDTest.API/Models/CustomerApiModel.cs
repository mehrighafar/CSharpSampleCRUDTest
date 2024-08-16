using System.ComponentModel.DataAnnotations;

namespace CSharpSampleCRUDTest.API.Models;

public class CustomerApiModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; } = new DateOnly();
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
}
