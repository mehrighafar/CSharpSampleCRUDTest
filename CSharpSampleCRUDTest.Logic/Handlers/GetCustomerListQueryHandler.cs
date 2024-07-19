using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Models;
using CSharpSampleCRUDTest.Logic.Queries;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Handlers;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerModel>>
{
    private readonly ICustomerDataAccessService _customerDataAccessService;
    public GetCustomerListQueryHandler(ICustomerDataAccessService customerDataAccessService)
    {
        _customerDataAccessService = customerDataAccessService;
    }
    public async Task<IEnumerable<CustomerModel>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        return await _customerDataAccessService.GetAllAsync();
    }
}
