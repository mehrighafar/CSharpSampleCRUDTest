using AutoMapper;
using CSharpSampleCRUDTest.API.MapperProfiles;
using CSharpSampleCRUDTest.API.Models;
using CSharpSampleCRUDTest.Domain.Interfaces.Services;
using CSharpSampleCRUDTest.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSharpSampleCRUDTest.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CustomerApiModelANDCustomerModelMapperProfile());
        });
        _mapper = new Mapper(config);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<CustomerApiModel> resultMapped;
        try
        {
            var result = await _customerService.GetAllAsync();
            if (result is null || result.Count() == 0)
            {
                // log

                return StatusCode(StatusCodes.Status204NoContent);
            }
            resultMapped = _mapper.Map<IEnumerable<CustomerApiModel>>(result);
        }
        catch
        {
            // log

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(resultMapped);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        CustomerApiModel? resultMapped;
        try
        {
            var result = await _customerService.GetByIdAsync(id);
            if (result is null)
            {
                // log

                return StatusCode(StatusCodes.Status204NoContent);
            }
            resultMapped = _mapper.Map<CustomerApiModel>(result);
        }
        catch
        {
            // log

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok(resultMapped);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CustomerApiModel model)
    {
        CustomerApiModel? resultMapped = null;

        try
        {
            var result = await _customerService.AddAsync(_mapper.Map<CustomerModel>(model));
            if (result is null)
                return StatusCode(StatusCodes.Status400BadRequest);

            resultMapped = _mapper.Map<CustomerApiModel>(result);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Created("~/", resultMapped);
    }
}
