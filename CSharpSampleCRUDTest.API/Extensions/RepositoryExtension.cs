using CSharpSampleCRUDTest.DataAccess.Repositories;

namespace CSharpSampleCRUDTest.API.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            // Add Customer collection
            services.AddScoped<ICustomerRepository, MongoCustomerRepository>();
            return services;
        }
    }
}
