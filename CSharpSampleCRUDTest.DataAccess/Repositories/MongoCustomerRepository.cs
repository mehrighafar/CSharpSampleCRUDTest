using CSharpSampleCRUDTest.DataAccess.Entities;
using URF.Core.Abstractions;

namespace CSharpSampleCRUDTest.DataAccess.Repositories;

public class MongoCustomerRepository : ICustomerRepository
{
    private readonly IDocumentRepository<CustomerEntity> _documentRepository;

    public MongoCustomerRepository(
        IDocumentRepository<CustomerEntity> documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        try
        {
            return await _documentRepository.FindManyAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerEntity?> GetByIdAsync(int id)
    {
        try
        {
            var x = await _documentRepository.FindOneAsync(e => e.Id == id);
            return x;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerEntity?> AddAsync(CustomerEntity entity)
    {
        var existing = await _documentRepository.FindOneAsync(e => e.Id == entity.Id);
        if (existing != null) return null;
        return await _documentRepository.InsertOneAsync(entity);
    }

    public async Task<CustomerEntity?> UpdateAsync(CustomerEntity entity)
    {
        var existing = await GetByIdAsync(entity.Id);
        if (existing == null) return null;
        return await _documentRepository.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity);
    }

    public async Task<int> RemoveAsync(int id) =>
        await _documentRepository.DeleteOneAsync(e => e.Id == id);
}

public class ConcurrencyException : Exception { }