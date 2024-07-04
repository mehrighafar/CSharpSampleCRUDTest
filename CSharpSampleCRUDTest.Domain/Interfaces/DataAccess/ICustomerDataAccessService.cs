﻿using CSharpSampleCRUDTest.Domain.Models;

namespace CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;

public interface ICustomerDataAccessService
{
    public Task<IEnumerable<CustomerModel>> GetAllAsync();
    public Task<CustomerModel> GetByIdAsync(int id);
    public Task<CustomerModel> AddAsync(CustomerModel model);
    public Task<CustomerModel> UpdateAsync(CustomerModel model);
    public Task<int> DeleteAsync(int id);
}