using AutoMapper;
using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.DataAccess.MapperProfiles;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Domain.Exceptions;
using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Models;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.DataAccess.DataAccessServices;

public class CustomerDataAccessService : ICustomerDataAccessService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerDataAccessService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CustomerEntityANDCustomerModelMapperProfile());
        });
        _mapper = new Mapper(config);
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        var result = await _customerRepository.GetAllAsync();
        if (result is null || result.Count() == 0) { return null; }

        return _mapper.Map<IEnumerable<CustomerModel>>(result);
    }

    public async Task<CustomerModel> GetByIdAsync(Guid id)
    {
        var result = await _customerRepository.GetByIdAsync(id);
        if (result is null) { throw new CustomerNotFoundException(id); }

        return _mapper.Map<CustomerModel>(result);
    }
    public async Task<CustomerModel> AddAsync(CustomerModel model)
    {
        var result = await _customerRepository.AddAsync(_mapper.Map<CustomerEntity>(model));
        if (result is null) { return null; }

        return _mapper.Map<CustomerModel>(result);
    }
    public async Task<CustomerModel> UpdateAsync(CustomerModel model)
    {
        var result = await _customerRepository.UpdateAsync(_mapper.Map<CustomerEntity>(model));
        if (result is null) { throw new CustomerNotFoundException(model.Id); }

        return model;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _customerRepository.RemoveAsync(id);
        return result.IsAcknowledged;
    }
}
