using System.ComponentModel.DataAnnotations;

namespace CSharpSampleCRUDTest.DataAccess.Entities;

public class CustomerEntity
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; } = new DateOnly();

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[0-9]+$")]
    public string BankAccountNumber { get; set; } = string.Empty;
}
