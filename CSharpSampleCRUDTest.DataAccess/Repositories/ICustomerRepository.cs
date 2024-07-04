using CSharpSampleCRUDTest.DataAccess.Entities;

namespace CSharpSampleCRUDTest.DataAccess.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerEntity>> GetAllAsync();
    Task<CustomerEntity?> GetByIdAsync(int id);
    Task<CustomerEntity?> AddAsync(CustomerEntity entity);
    Task<CustomerEntity?> UpdateAsync(CustomerEntity entity);
    Task<int> RemoveAsync(int id);
}