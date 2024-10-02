using CSharpSampleCRUDTest.API.Extensions;
using CSharpSampleCRUDTest.API.Middleware;
using CSharpSampleCRUDTest.DataAccess.DataAccessServices;
using CSharpSampleCRUDTest.Domain.Interfaces.DataAccess;
using CSharpSampleCRUDTest.Domain.Interfaces.Services;
using CSharpSampleCRUDTest.Logic;
using CSharpSampleCRUDTest.Logic.PipelineBehaviors;
using CSharpSampleCRUDTest.Logic.Services;
using dotenv.net;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerDataAccessService, CustomerDataAccessService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Register repository   
builder.Services.AddMongoDatabase();
builder.Services.AddRepository();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

public partial class Program { }
