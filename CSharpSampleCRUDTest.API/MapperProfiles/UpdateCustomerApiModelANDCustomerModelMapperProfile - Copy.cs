using AutoMapper;
using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.Domain.Models;

namespace CSharpSampleCRUDTest.API.MapperProfiles;

public class UpdateCustomerApiModelANDCustomerModelMapperProfile : Profile
{
    public UpdateCustomerApiModelANDCustomerModelMapperProfile()
    {
        CreateMap<CustomerModel, UpdateCustomerApiModel>().ReverseMap();
    }
}
