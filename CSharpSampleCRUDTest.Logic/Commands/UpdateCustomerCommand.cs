using CSharpSampleCRUDTest.Domain.Models;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Commands;

public record UpdateCustomerCommand(CustomerModel NewModel) : IRequest<CustomerModel>;
