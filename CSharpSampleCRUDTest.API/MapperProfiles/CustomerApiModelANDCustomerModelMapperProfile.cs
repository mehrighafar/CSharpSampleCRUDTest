using AutoMapper;
using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.Domain.Models;

namespace CSharpSampleCRUDTest.API.MapperProfiles;

public class CustomerApiModelANDCustomerModelMapperProfile : Profile
{
    public CustomerApiModelANDCustomerModelMapperProfile()
    {
        CreateMap<CustomerModel, CustomerApiModel>().ReverseMap();
    }
}
