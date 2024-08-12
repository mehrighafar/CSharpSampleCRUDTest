using BoDi;
using CSharpSampleCRUDTest.API.Configuration;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Test.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace CSharpSampleCRUDTest.Test.BDD.Hooks;

[Binding]
public class CustomerHooks
{
    private readonly IObjectContainer _objectContainer;
    private const string AppSettingsFile = "appsettings.json";

    public CustomerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario]
    public async Task RegisterServices()
    {
        var factory = GetWebApplicationFactory();
        await ClearData(factory);
        _objectContainer.RegisterInstanceAs(factory);
        var jsonFilesRepo = new JsonFilesRepository();
        _objectContainer.RegisterInstanceAs(jsonFilesRepo);
        var repository = (ICustomerRepository)factory.Services.GetService(typeof(ICustomerRepository))!;
        _objectContainer.RegisterInstanceAs(repository);
    }

    private WebApplicationFactory<Program> GetWebApplicationFactory() =>
        new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                IConfigurationSection? configSection = null;
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFile));
                    configSection = context.Configuration.GetSection(nameof(CSharpSampleCRUDTestDatabaseSettings));
                });
                builder.ConfigureTestServices(services =>
                    services.Configure<CSharpSampleCRUDTestDatabaseSettings>(configSection));
            });

    private async Task ClearData(
        WebApplicationFactory<Program> factory)
    {
        if (factory.Services.GetService(typeof(ICustomerRepository))
            is not ICustomerRepository repository) return;
        var entities = await repository.GetAllAsync();
        foreach (var entity in entities)
            await repository.RemoveAsync(entity.Id);
    }
}