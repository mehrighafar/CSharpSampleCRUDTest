using MediatR;

namespace CSharpSampleCRUDTest.Logic.Commands;

public record DeleteCustomerCommand(Guid Id) : IRequest<bool>;
