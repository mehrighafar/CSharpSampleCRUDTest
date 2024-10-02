using CSharpSampleCRUDTest.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpSampleCRUDTest.Test.Repositories.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            // Add Customer collection
            services.AddSingleton<ICustomerRepository, MongoCustomerRepository>();
            return services;
        }
    }
}
