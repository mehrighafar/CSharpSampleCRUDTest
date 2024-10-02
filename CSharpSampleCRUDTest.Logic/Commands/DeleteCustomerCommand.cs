using MediatR;

namespace CSharpSampleCRUDTest.Logic.Commands;

public record DeleteCustomerCommand(int Id) : IRequest<int>;
