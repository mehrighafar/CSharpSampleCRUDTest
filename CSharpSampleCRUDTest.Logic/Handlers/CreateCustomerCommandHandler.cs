using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Models;
using CSharpSampleCRUDTest.Logic.Commands;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerModel>
{
    private readonly ICustomerDataAccessService _customerDataAccessService;
    public CreateCustomerCommandHandler(ICustomerDataAccessService customerDataAccessService)
    {
        _customerDataAccessService = customerDataAccessService;
    }
    public async Task<CustomerModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = await _customerDataAccessService.AddAsync(request.NewModel);

        if (result is null)
            throw new Exception("An error while processing the request occured.");

        return result;
    }
}
