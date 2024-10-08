using AutoMapper;
using CSharpSampleCRUDTest.API.MapperProfiles;
using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.Domain.Models;
using CSharpSampleCRUDTest.Logic.Commands;
using CSharpSampleCRUDTest.Logic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSharpSampleCRUDTest.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Mapper _updateApiMapper;
    private readonly Mapper _apiMapper;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;

        var configUpdateApiMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UpdateCustomerApiModelANDCustomerModelMapperProfile());
        });
        _updateApiMapper = new Mapper(configUpdateApiMapper);

        var configApiMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CustomerApiModelANDCustomerModelMapperProfile());
        });
        _apiMapper = new Mapper(configApiMapper);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UpdateCustomerApiModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetCustomerListQuery());
        if (result is null || result.Count() == 0)
            return StatusCode(StatusCodes.Status204NoContent);

        var resultMapped = _updateApiMapper.Map<IEnumerable<UpdateCustomerApiModel>>(result);

        return Ok(resultMapped);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(UpdateCustomerApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id));

        var resultMapped = _updateApiMapper.Map<UpdateCustomerApiModel>(result);

        return Ok(resultMapped);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UpdateCustomerApiModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] CustomerModel model)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(_apiMapper.Map<CustomerModel>(model)));

        var resultMapped = _updateApiMapper.Map<UpdateCustomerApiModel>(result);

        return Created("~/", resultMapped);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCustomerApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerApiModel model)
    {
        var result = await _mediator.Send(new UpdateCustomerCommand(_updateApiMapper.Map<CustomerModel>(model)));

        var resultMapped = _updateApiMapper.Map<UpdateCustomerApiModel>(result);

        return Ok(resultMapped);
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteCustomerCommand(id));

        return NoContent();
    }
}
