using AutoMapper;
using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.Domain.Models;

namespace CSharpSampleCRUDTest.DataAccess.MapperProfiles;

public class CustomerEntityANDCustomerModelMapperProfile : Profile
{
    public CustomerEntityANDCustomerModelMapperProfile()
    {
        CreateMap<CustomerModel, CustomerEntity>().ReverseMap();
    }
}
