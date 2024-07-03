using EventDriven.DependencyInjection.URF.Mongo;

namespace CSharpSampleCRUDTest.API.Configuration;

public class CSharpSampleCRUDTestDatabaseSettings : IMongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
}