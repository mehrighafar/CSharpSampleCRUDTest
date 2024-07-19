using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Models;
using CSharpSampleCRUDTest.Logic.Commands;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerModel>
{
    private readonly ICustomerDataAccessService _customerDataAccessService;
    public UpdateCustomerCommandHandler(ICustomerDataAccessService customerDataAccessService)
    {
        _customerDataAccessService = customerDataAccessService;
    }
    public async Task<CustomerModel> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await _customerDataAccessService.UpdateAsync(request.NewModel);

        if (result is null)
            throw new Exception("An error while processing the request occured.");

        return result;
    }
}
