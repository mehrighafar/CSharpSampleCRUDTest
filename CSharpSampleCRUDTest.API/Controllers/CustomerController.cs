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
    private readonly Mapper _mapper;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CustomerApiModelANDCustomerModelMapperProfile());
        });
        _mapper = new Mapper(config);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerApiModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetCustomerListQuery());
        if (result is null || result.Count() == 0)
            return StatusCode(StatusCodes.Status204NoContent);

        var resultMapped = _mapper.Map<IEnumerable<CustomerApiModel>>(result);

        return Ok(resultMapped);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IEnumerable<CustomerApiModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)

    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id));

        var resultMapped = _mapper.Map<CustomerApiModel>(result);

        return Ok(resultMapped);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerApiModel), StatusCodes.Status201Created)]
    public async Task<IActionResult> Add([FromBody] CustomerApiModel model)
    {
        var result = await _mediator.Send(new CreateCustomerCommand(_mapper.Map<CustomerModel>(model)));

        var resultMapped = _mapper.Map<CustomerApiModel>(result);

        return Created("~/", resultMapped);
    }

    [HttpPut]
    [ProducesResponseType(typeof(CustomerApiModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromBody] CustomerApiModel model)
    {
        var result = await _mediator.Send(new UpdateCustomerCommand(_mapper.Map<CustomerModel>(model)));

        var resultMapped = _mapper.Map<CustomerApiModel>(result);

        return Ok(resultMapped);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)

    {
        await _mediator.Send(new DeleteCustomerCommand(id));

        return NoContent();
    }
}
