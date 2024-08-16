using AutoMapper;
using CSharpSampleCRUDTest.API.MapperProfiles;
using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.Domain.Interfaces.Services;
using CSharpSampleCRUDTest.Domain.Models;
using CSharpSampleCRUDTest.Logic.Commands;
using CSharpSampleCRUDTest.Logic.Handlers;
using CSharpSampleCRUDTest.Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpSampleCRUDTest.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerService _customerService;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IMapper _updateMapper;

    public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService, IMediator mediator)
    {
        _logger = logger;
        _customerService = customerService;
        _mediator = mediator;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CustomerApiModelANDCustomerModelMapperProfile());
        });
        _mapper = new Mapper(config);

        var updateConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UpdateCustomerApiModelANDCustomerModelMapperProfile());
        });
        _updateMapper = new Mapper(updateConfig);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<UpdateCustomerApiModel> resultMapped;

        var result = await _mediator.Send(new GetCustomerListQuery());
        if (result is null || result.Count() == 0)
        {
            // log

            return StatusCode(StatusCodes.Status204NoContent);
        }

        try
        {
            resultMapped = _updateMapper.Map<IEnumerable<UpdateCustomerApiModel>>(result);
            return Ok(resultMapped);
        }
        catch
        {
            //log

            throw new Exception("An error while processing the request occured.");
        }
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        UpdateCustomerApiModel? resultMapped;

        var result = await _mediator.Send(new GetCustomerByIdQuery(id));

        try
        {
            resultMapped = _updateMapper.Map<UpdateCustomerApiModel>(result);
            return Ok(resultMapped);
        }
        catch
        {
            // log

            throw new Exception("An error while processing the request occured.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CustomerApiModel model)
    {
        UpdateCustomerApiModel? resultMapped = null;

        var result = await _mediator.Send(new CreateCustomerCommand(_mapper.Map<CustomerModel>(model)));

        try
        {
            resultMapped = _updateMapper.Map<UpdateCustomerApiModel>(result);
            return Created("~/", resultMapped);
        }
        catch
        {
            throw new Exception("An error while processing the request occured.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerApiModel model)
    {
        UpdateCustomerApiModel? resultMapped = null;

        var result = await _mediator.Send(new UpdateCustomerCommand(_updateMapper.Map<CustomerModel>(model)));

        try
        {
            resultMapped = _updateMapper.Map<UpdateCustomerApiModel>(result);
            return Ok(resultMapped);
        }
        catch
        {
            throw new Exception("An error while processing the request occured.");
        }
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteCustomerCommand(id));

        try
        {
            return NoContent();
        }
        catch
        {
            throw new Exception("An error while processing the request occured.");
        }
    }
}
