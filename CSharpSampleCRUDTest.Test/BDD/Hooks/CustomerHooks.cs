using BoDi;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Test.Repositories;
using CSharpSampleCRUDTest.Test.Repositories.Extensions;
using dotenv.net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;

namespace CSharpSampleCRUDTest.Test.BDD.Hooks;

[Binding]
public class CustomerHooks(IObjectContainer objectContainer)
{
    private readonly IObjectContainer _objectContainer = objectContainer;

    [BeforeScenario]
    public async Task RegisterServices()
    {
        DotEnv.Load();

        var factory = GetWebApplicationFactory();
        await ClearData(factory);
        _objectContainer.RegisterInstanceAs(factory);
        var jsonFilesRepo = new JsonFilesRepository();
        _objectContainer.RegisterInstanceAs(jsonFilesRepo);
        var repository = (ICustomerRepository)factory.Services.GetService(typeof(ICustomerRepository))!;
        _objectContainer.RegisterInstanceAs(repository);
    }

    private static WebApplicationFactory<Program> GetWebApplicationFactory() =>
        new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    DotEnv.Load();

                    config.AddEnvironmentVariables();
                });
                builder.ConfigureTestServices(services =>
                {
                    services.AddMongoDatabase();
                    services.AddRepository();
                });
            });

    private static async Task ClearData(
        WebApplicationFactory<Program> factory)
    {
        if (factory.Services.GetService(typeof(ICustomerRepository))
            is not ICustomerRepository repository) return;
        var entities = await repository.GetAllAsync();
        foreach (var entity in entities)
            await repository.RemoveAsync(entity.Id);
    }
}