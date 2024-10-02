using CSharpSampleCRUDTest.Logic.Commands;
using FluentValidation;
using PhoneNumbers;

namespace CSharpSampleCRUDTest.Logic.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(command => command.NewModel.FirstName)
            .NotEmpty()
            .WithMessage("The customer FirstName cannot be empty.");

        RuleFor(command => command.NewModel.LastName)
            .NotEmpty()
            .WithMessage("The customer LastName cannot be empty.");

        RuleFor(command => command.NewModel.DateOfBirth)
            .NotEmpty()
            .WithMessage("The customer DateOfBirth cannot be empty.");

        RuleFor(command => command.NewModel.PhoneNumber)
            .NotEmpty()
            .WithMessage("The customer PhoneNumber cannot be empty.");

        RuleFor(command => command.NewModel.PhoneNumber)
            .Must(IsValidNumber)
            .WithMessage("The customer PhoneNumber must be valid.");

        RuleFor(command => command.NewModel.Email)
            .NotEmpty()
            .WithMessage("The customer Email cannot be empty.");

        RuleFor(command => command.NewModel.Email)
            .EmailAddress()
            .WithMessage("The customer Email must be valid.");

        RuleFor(command => command.NewModel.BankAccountNumber)
            .NotEmpty()
            .WithMessage("The customer BankAccountNumber cannot be empty.");

        RuleFor(command => command.NewModel.BankAccountNumber)
            .CreditCard()
            .WithMessage("The customer BankAccountNumber must be valid.");
    }

    private bool IsValidNumber(string number)
    {
        if (!number.All(char.IsDigit))
            return false;

        PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        PhoneNumber phoneNumber = phoneNumberUtil.Parse(number, "NL");
        var result = phoneNumberUtil.IsValidNumber(phoneNumber);
        return result;
    }
}