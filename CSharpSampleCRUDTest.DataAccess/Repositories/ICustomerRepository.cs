using CSharpSampleCRUDTest.DataAccess.Entities;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.DataAccess.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerEntity>> GetAllAsync();
    Task<CustomerEntity?> GetByIdAsync(Guid id);
    Task<CustomerEntity?> AddAsync(CustomerEntity entity);
    Task<CustomerEntity?> UpdateAsync(CustomerEntity entity);
    Task<DeleteResult> RemoveAsync(Guid id);
}