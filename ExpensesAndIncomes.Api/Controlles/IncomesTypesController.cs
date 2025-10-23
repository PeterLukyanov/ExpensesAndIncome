using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

//This controller processes requests: show all income types,
// show all information about a specific type, display the total amount of incomes,
// add a new income type, update an income type, delete an income type


[ApiController]
[Route("[controller]")]
public class IncomesTypesController : ControllerBase
{
    private readonly IncomesTypesService _incomesTypesManipulator;
    private readonly ILogger<IncomesTypesController> _logger;
    public IncomesTypesController(IncomesTypesService incomesTypeManipulator, ILogger<IncomesTypesController> logger)
    {
        _incomesTypesManipulator = incomesTypeManipulator;
        _logger = logger;
    }

    //This query returns a list of all income types.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("TypesOfIncomes")]
    public async Task<ActionResult<List<string>>> GetAll()
    {
        _logger.LogInformation("Calling a service to execute all incomes");
        var result = await _incomesTypesManipulator.InfoTypes();
        if (result.IsFailure)
        {
            _logger.LogWarning("Operation fail");
            return NotFound(result.Error);
        }
        _logger.LogInformation("Operation success");
        return Ok(result.Value);
    }

    //This query displays all information on the specified income type: a list of all incomes for the selected type,
    //  the type name, the total income amount for the type
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<TypeOfOperationsForOutputDto<Income>>> GetByType(int id)
    {
        _logger.LogInformation("Calling a service to execute type of expenses by id");
        var result = await _incomesTypesManipulator.GetInfoOfType(id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Operation fail");
            return BadRequest(result.Error);
        }
        _logger.LogInformation("Operation success");
        return Ok(result.Value);
    }

    //This query returns the total amount of the income.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("TotalSumOfIncomes")]
    public async Task<ActionResult<double>> GetTotalSumOfIncomes()
    {
        _logger.LogInformation("Calling a service to execute total sum of all incomes");
        var result = await _incomesTypesManipulator.TotalSumOfOperations();
        if (result.IsSuccess)
        {
            _logger.LogInformation("Operation success");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Operation fail");
            return NotFound(result.Error);
        }
    }

    //This request adds a new income type.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpPost]
    public async Task<IActionResult> AddType([FromBody] NameTypeOfOperationsDto typeOfIncomes)
    {
        _logger.LogInformation("Calling a service to add new type of incomes");
        var result = await _incomesTypesManipulator.AddType(typeOfIncomes);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Operation success");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Operation fail");
            return BadRequest(result.Error);
        }
    }

    //This request is to update the name of the income type, while overwriting all income objects marked with this income type.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update([FromBody] NameTypeOfOperationsDto listOfIncomes, int id)
    {
        _logger.LogInformation("Calling a service to update type of incomes by id");
        var result = await _incomesTypesManipulator.Update(listOfIncomes, id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Operation fail");
            return NotFound(result.Error);
        }
        else
        {
            _logger.LogInformation("Operation success");
            return Ok(result.Value);
        }
    }

    //This request is to delete an income type, which will delete all income objects that are marked with this income type.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Calling a service to delete type of incomes with {id} id and all incomes from this type");
        var result = await _incomesTypesManipulator.Delete(id);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Operation success");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Operation fail");
            return BadRequest(result.Error);
        }
    }
}