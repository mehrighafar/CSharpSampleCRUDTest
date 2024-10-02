using CSharpSampleCRUDTest.Domain.Models;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Commands;

public record CreateCustomerCommand(CustomerModel NewModel) : IRequest<CustomerModel>;
