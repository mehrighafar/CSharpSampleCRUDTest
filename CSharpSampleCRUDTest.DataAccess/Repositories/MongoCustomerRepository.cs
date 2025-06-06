using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.Domain.Exceptions;
using MongoDB.Driver;

namespace CSharpSampleCRUDTest.DataAccess.Repositories;

public class MongoCustomerRepository : ICustomerRepository
{
    private readonly IMongoCollection<CustomerEntity> _mongoCollection;

    public MongoCustomerRepository(IMongoDatabase mongoDatabase)
        => _mongoCollection = mongoDatabase.GetCollection<CustomerEntity>(nameof(CustomerEntity));

    public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        var filter = Builders<CustomerEntity>.Filter.Empty;
        return await _mongoCollection.Find(filter).ToListAsync(); ;
    }

    public async Task<CustomerEntity?> GetByIdAsync(Guid id)
    {
        var x = await _mongoCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        return x;
    }

    public async Task<CustomerEntity> AddAsync(CustomerEntity entity)
    {
        if (!await IsEmailUniqueInDb(entity.Id, entity.Email))
            return null;
        else if (!await IsFirstnameLastnameDateofbirthUnique
            (entity.Id, entity.FirstName, entity.LastName, entity.DateOfBirth))
            return null;

        entity.Id = Guid.NewGuid();
        await _mongoCollection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<CustomerEntity?> UpdateAsync(CustomerEntity entity)
    {
        if (!await IsEmailUniqueInDb(entity.Id, entity.Email))
            return null;
        else if (!await IsFirstnameLastnameDateofbirthUnique
            (entity.Id, entity.FirstName, entity.LastName, entity.DateOfBirth))
            return null;

        var existing = await GetByIdAsync(entity.Id) ?? throw new CustomerNotFoundException(entity.Id);

        return await _mongoCollection.FindOneAndReplaceAsync(e => e.Id == entity.Id, entity);
    }

    public async Task<DeleteResult> RemoveAsync(Guid id) =>
        await _mongoCollection.DeleteOneAsync(e => e.Id == id);

    private async Task<bool> IsEmailUniqueInDb(Guid id, string email)
    {
        var filter =
            Builders<CustomerEntity>.Filter.Ne(u => u.Id, id) &
            Builders<CustomerEntity>.Filter.Eq(u => u.Email, email);

        var existing = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        return existing == null;
    }

    private async Task<bool> IsFirstnameLastnameDateofbirthUnique(Guid id, string firstName, string lastName, DateOnly dateOfBirth)
    {
        var filter =
            Builders<CustomerEntity>.Filter.Ne(u => u.Id, id) &
            Builders<CustomerEntity>.Filter.Eq(u => u.FirstName, firstName) &
            Builders<CustomerEntity>.Filter.Eq(u => u.LastName, lastName) &
            Builders<CustomerEntity>.Filter.Eq(u => u.DateOfBirth, dateOfBirth);


        var existing = await _mongoCollection.Find(filter).FirstOrDefaultAsync();
        return existing == null;
    }
}
