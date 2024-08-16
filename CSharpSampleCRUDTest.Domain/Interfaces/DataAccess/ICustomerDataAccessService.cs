using CSharpSampleCRUDTest.Domain.Models;

namespace CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;

public interface ICustomerDataAccessService
{
    public Task<IEnumerable<CustomerModel>> GetAllAsync();
    public Task<CustomerModel> GetByIdAsync(Guid id);
    public Task<CustomerModel> AddAsync(CustomerModel model);
    public Task<CustomerModel> UpdateAsync(CustomerModel model);
    public Task<bool> DeleteAsync(Guid id);
}
