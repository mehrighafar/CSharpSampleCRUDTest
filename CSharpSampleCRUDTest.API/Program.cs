using CSharpSampleCRUDTest.API.Configuration;
using CSharpSampleCRUDTest.DataAccess.DataAccessServices;
using CSharpSampleCRUDTest.DataAccess.Entities;
using CSharpSampleCRUDTest.DataAccess.Repositories;
using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Interfaces.Services;
using CSharpSampleCRUDTest.Logic.Services;
using EventDriven.DependencyInjection.URF.Mongo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerDataAccessService, CustomerDataAccessService>();

// Register repository
builder.Services.AddSingleton<ICustomerRepository, MongoCustomerRepository>();
builder.Services.AddMongoDbSettings<CSharpSampleCRUDTestDatabaseSettings, CustomerEntity>(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }
