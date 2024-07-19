using CSharpSampleCRUDTest.Domain.Exceptions;
using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Models;
using CSharpSampleCRUDTest.Logic.Queries;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Handlers;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerModel>
{
    private readonly ICustomerDataAccessService _customerDataAccessService;
    public GetCustomerByIdQueryHandler(ICustomerDataAccessService customerDataAccessService)
    {
        _customerDataAccessService = customerDataAccessService;
    }
    public async Task<CustomerModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _customerDataAccessService.GetByIdAsync(request.Id);

        if (result is null)
            throw new CustomerNotFoundException(request.Id);

        return result;
    }
}
