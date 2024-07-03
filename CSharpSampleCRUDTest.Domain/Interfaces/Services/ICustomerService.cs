using CSharpSampleCRUDTest.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSampleCRUDTest.Domain.Interfaces.Services;

public interface ICustomerService
{
    public Task<IEnumerable<CustomerModel>> GetAllAsync();
    public Task<CustomerModel> GetByIdAsync(int id);
    public Task<CustomerModel> AddAsync(CustomerModel model);
    public Task<CustomerModel> UpdateAsync(CustomerModel model);
    public Task<int> DeleteAsync(int id);
}
