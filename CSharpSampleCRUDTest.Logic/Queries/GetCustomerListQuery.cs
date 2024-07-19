using CSharpSampleCRUDTest.Domain.Models;
using MediatR;

namespace CSharpSampleCRUDTest.Logic.Queries;

public class GetCustomerListQuery() : IRequest<IEnumerable<CustomerModel>>;
