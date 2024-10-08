﻿using System.ComponentModel.DataAnnotations;

namespace CSharpSampleCRUDTest.DataAccess.Entities;

public class CustomerEntity
{
    [Required]
    public Guid Id { get; set; }

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
    public string Email { get; set; } = string.Empty;

    [Required]
    public string BankAccountNumber { get; set; } = string.Empty;
}
