using CSharpSampleCRUDTest.API;
using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Test.Helpers;
using CSharpSampleCRUDTest.Test.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CSharpSampleCRUDTest.Test.Steps;

[Binding]
public sealed class CustomerWebApiStepDefinitions
{
    private const string BaseAddress = "http://localhost:5111";
    public WebApplicationFactory<Program> Factory { get; }
    public ICustomerRepository Repository { get; }
    public HttpClient Client { get; set; } = null!;
    private readonly ScenarioContext _scenarioContext;
    public JsonFilesRepository JsonFilesRepo { get; }

    private JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true
    };

    public CustomerWebApiStepDefinitions(
        WebApplicationFactory<Program> factory,
        ICustomerRepository repository,
        JsonFilesRepository jsonFilesRepo,
        ScenarioContext scenarioContext)
    {
        Factory = factory;
        Repository = repository;
        JsonFilesRepo = jsonFilesRepo;
        _scenarioContext = scenarioContext;
    }

    [Given(@"I am a client")]
    public void GivenIAmAClient()
    {
        Client = Factory.CreateDefaultClient(new Uri(BaseAddress));
    }

    [Given(@"The repository has customer data")]
    public async Task GivenTheRepositoryHasCustomerData()
    {
        var cutomersJson = JsonFilesRepo.Files["customers.json"];
        var cutomers = JsonSerializer.Deserialize<IList<CustomerEntity>>(cutomersJson, JsonSerializerOptions);
        if (cutomers != null)
            foreach (var cutomer in cutomers)
                await Repository.AddAsync(cutomer);
    }

    /// <summary>
    /// Scenario 1
    /// </summary>
    /// 
    [When(@"I make a GET request to '(.*)'")]
    public async Task WhenIMakeAGetRequestTo(string endpoint)
    {
        _scenarioContext.Add("GetCustomersResponse", await Client.GetAsync(endpoint));
    }

    [Then(@"The response for get status code is 200")]
    public void ThenTheResponseStatusCodeIs()
    {
        _scenarioContext.Get<HttpResponseMessage>("GetCustomersResponse").StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"The response for get json should be '(.*)'")]
    public async Task ThenTheResponseForGetJsonShouldBe(string file)
    {
        var expected = JsonFilesRepo.Files[file];
        var response = await _scenarioContext.Get<HttpResponseMessage>("GetCustomersResponse").Content.ReadAsStringAsync();
        var actual = response.JsonPrettify();
        Assert.That(expected, Is.EqualTo(actual));
    }

    /// <summary>
    /// Scenario 2
    /// </summary>
    [When(@"I make a POST request with '(.*)' to '(.*)'")]
    public async Task WhenIMakeAPostRequestWithTo(string file, string endpoint)
    {
        var json = JsonFilesRepo.Files[file];
        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        _scenarioContext.Add("AddCustomerResponse", await Client.PostAsync(endpoint, content));
    }

    [Then(@"The '(.*)' is created successfully")]
    public async Task ThenTheCustomerIsCreatedSuccessfully(string endpoint)
    {
        var expected = await _scenarioContext.Get<HttpResponseMessage>("AddCustomerResponse").Content.ReadAsStringAsync();
        var expectedJsonPrettified = expected.JsonPrettify();
        var expectedJson = JsonSerializer.Deserialize<CustomerApiModel>(expected, JsonSerializerOptions);

        if (expectedJson is null || expectedJson.Id < 1) { Assert.Fail(); }

        var result = await Client.GetAsync(endpoint + $"/{expectedJson!.Id}");
        var resultContent = await result.Content.ReadAsStringAsync();
        var resultJson = resultContent.JsonPrettify();

        Assert.That(expectedJsonPrettified, Is.EqualTo(resultJson));
    }

    [Then(@"The response for creation status code is 201")]
    public void ThenTheResponseForCreationStatusCodeIs()
    {
        _scenarioContext.Get<HttpResponseMessage>("AddCustomerResponse").StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Then(@"The response for creation json should be '(.*)'")]
    public async Task ThenTheResponseForCreationJsonShouldBe(string file)
    {
        var expected = JsonFilesRepo.Files[file];
        var response = await _scenarioContext.Get<HttpResponseMessage>("AddCustomerResponse").Content.ReadAsStringAsync();
        var actual = response.JsonPrettify();
        Assert.That(expected, Is.EqualTo(actual));
    }

    /// <summary>
    /// Scenario 3
    /// </summary>
    [When(@"I make a PUT request with '(.*)' to '(.*)'")]
    public async Task WhenIMakeAPutRequestWithTo(string file, string endpoint)
    {
        var json = JsonFilesRepo.Files[file];
        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        _scenarioContext.Add("UpdateCustomerResponse", await Client.PutAsync(endpoint, content));
    }

    [Then(@"The response for update status code is 200")]
    public void ThenTheResponseForUpdateStatusCodeIs()
    {
        _scenarioContext.Get<HttpResponseMessage>("UpdateCustomerResponse").StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"The response for update json should be '(.*)'")]
    public async Task ThenTheResponseForUpdateJsonShouldBe(string file)
    {
        var expected = JsonFilesRepo.Files[file];
        var response = await _scenarioContext.Get<HttpResponseMessage>("UpdateCustomerResponse").Content.ReadAsStringAsync();
        var actual = response.JsonPrettify();
        Assert.That(expected, Is.EqualTo(actual));
    }
}