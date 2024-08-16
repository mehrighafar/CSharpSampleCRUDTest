using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Test.Entities;
using CSharpSampleCRUDTest.Test.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CSharpSampleCRUDTest.Test.BDD.Steps;

[Binding]
public sealed class CustomerWebApiStepDefinitions
{
    private string BaseAddress;
    public WebApplicationFactory<Program> Factory { get; }
    public ICustomerRepository Repository { get; }
    public HttpClient Client { get; set; } = null!;
    private readonly ScenarioContext _scenarioContext;
    public JsonFilesRepository JsonFilesRepo { get; }

    private JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
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
        BaseAddress = Environment.GetEnvironmentVariable("API_BASE_ADDRESS")!;
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
        var expectedEntities = JsonSerializer.Deserialize<IEnumerable<TestCustomerEntity>>(expected, JsonSerializerOptions);

        var response = await _scenarioContext.Get<HttpResponseMessage>("GetCustomersResponse").Content.ReadAsStringAsync();
        var actualEntities = JsonSerializer.Deserialize<IEnumerable<TestCustomerEntity>>(response, JsonSerializerOptions);

        actualEntities.Should().BeEquivalentTo(expectedEntities);
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
        var expectedEntity = JsonSerializer.Deserialize<UpdateCustomerApiModel>(expected, JsonSerializerOptions);

        if (expectedEntity is null) { Assert.Fail(); }

        var result = await Client.GetAsync(endpoint + $"/{expectedEntity!.Id}");
        var resultContent = await result.Content.ReadAsStringAsync();
        var resultEntity = JsonSerializer.Deserialize<UpdateCustomerApiModel>(resultContent, JsonSerializerOptions);

        resultEntity.Should().BeEquivalentTo(expectedEntity);
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
        var expectedEntity = JsonSerializer.Deserialize<TestCustomerEntity>(expected, JsonSerializerOptions);

        var actual = await _scenarioContext.Get<HttpResponseMessage>("AddCustomerResponse").Content.ReadAsStringAsync();
        var actualEntity = JsonSerializer.Deserialize<TestCustomerEntity>(actual, JsonSerializerOptions);

        actualEntity.Should().BeEquivalentTo(expectedEntity);
    }

    /// <summary>
    /// Scenario 3
    /// </summary>
    [When(@"I make a PUT request for a customer got from '(.*)' to '(.*)'")]
    public async Task WhenIMakeAPutRequestWithTo(string getEndpoint, string putEndpoint)
    {
        // Get a customer
        var response = await Client.GetAsync(getEndpoint);
        var content = await response.Content.ReadAsStringAsync();
        var customer = JsonSerializer.Deserialize<IEnumerable<UpdateCustomerApiModel>>(content, JsonSerializerOptions)!.FirstOrDefault();

        // Update a customer
        customer!.FirstName = "updatedCustomerFirstName";
        _scenarioContext.Add("UpdatedCustomer", customer);
        var updateCustomer = JsonSerializer.Serialize(customer, JsonSerializerOptions);
        var updateContent = new StringContent(updateCustomer, Encoding.UTF8, MediaTypeNames.Application.Json);

        _scenarioContext.Add("UpdateCustomerResponse", await Client.PutAsync(putEndpoint, updateContent));
    }

    [Then(@"The response for update status code is 200")]
    public void ThenTheResponseForUpdateStatusCodeIs()
    {
        _scenarioContext.Get<HttpResponseMessage>("UpdateCustomerResponse").StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"The response for update json should be the same sent")]
    public async Task ThenTheResponseForUpdateJsonShouldBe()
    {
        var expectedEntity = _scenarioContext.Get<UpdateCustomerApiModel>("UpdatedCustomer");

        var actual = await _scenarioContext.Get<HttpResponseMessage>("UpdateCustomerResponse").Content.ReadAsStringAsync();
        var actualEntity = JsonSerializer.Deserialize<UpdateCustomerApiModel>(actual, JsonSerializerOptions);

        actualEntity.Should().BeEquivalentTo(expectedEntity);
    }

    /// <summary>
    /// Scenario 4
    /// </summary>
    [When(@"I make a DELETE request for a customer got from '(.*)' to '(.*)'")]
    public async Task WhenIMakeADeleteRequestWithIdTo(string getEndpoint, string deleteEndpoint)
    {
        // Get a customer
        var response = await Client.GetAsync(getEndpoint);
        var content = await response.Content.ReadAsStringAsync();
        var customer = JsonSerializer.Deserialize<IEnumerable<UpdateCustomerApiModel>>(content, JsonSerializerOptions)!.FirstOrDefault();

        // Delete a customer
        _scenarioContext.Add("DeleteCustomerResponse", await Client.DeleteAsync($"{deleteEndpoint}/{customer!.Id}"));
    }

    [Then(@"The response for delete status code is 204")]
    public void ThenTheResponseForDeleteStatusCodeIs()
    {
        _scenarioContext.Get<HttpResponseMessage>("DeleteCustomerResponse").StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}