using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.Domain.Exceptions;
using MongoDB.Driver;

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
        return await _documentRepository.FindManyAsync();
    }

    public async Task<CustomerEntity?> GetByIdAsync(int id)
    {
        var x = await _documentRepository.FindOneAsync(e => e.Id == id);
        return x;
    }

    public async Task<CustomerEntity?> AddAsync(CustomerEntity entity)
    {
        var existing = await _documentRepository.FindOneAsync(e => e.Id == entity.Id);
        if (existing != null)
            throw new CustomerExistsException(entity.Id);

        return await _documentRepository.InsertOneAsync(entity);
    }

    public async Task<CustomerEntity?> UpdateAsync(CustomerEntity entity)
    {
        var existing = await GetByIdAsync(entity.Id);
        if (existing == null)
            throw new CustomerNotFoundException(entity.Id);

        return await _documentRepository.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity);
    }

    public async Task<DeleteResult> RemoveAsync(Guid id) =>
        await _mongoCollection.DeleteOneAsync(e => e.Id == id);

}
