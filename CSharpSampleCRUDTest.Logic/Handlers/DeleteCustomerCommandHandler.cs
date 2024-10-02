using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Logic.Commands;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Handlers;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, int>
{
    private readonly ICustomerDataAccessService _customerDataAccessService;
    public DeleteCustomerCommandHandler(ICustomerDataAccessService customerDataAccessService)
    {
        _customerDataAccessService = customerDataAccessService;
    }
    public async Task<int> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await _customerDataAccessService.DeleteAsync(request.Id);

        if (result is 0)
            throw new Exception("An error while processing the request occured.");

        return result;
    }
}
