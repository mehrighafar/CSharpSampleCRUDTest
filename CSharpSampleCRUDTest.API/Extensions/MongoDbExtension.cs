using MongoDB.Driver;

namespace CSharpSampleCRUDTest.API.Extensions
{
    public static class MongoDbExtension
    {
        public static IServiceCollection AddMongoDatabase(this IServiceCollection services)
        {
            var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
                ?? throw new InvalidOperationException("Connection string is not set."));
            var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
                ?? throw new InvalidOperationException("DatabasenName is not set."));
            services.AddScoped(provider => mongoDatabase);
            return services;
        }
    }
}
