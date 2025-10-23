using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace Controllers;

//This controller processes requests: show all expense types,
// show all information about a specific type, display the total amount of expenses,
// add a new expense type, update an expense type, delete an expense type

[ApiController]
[Route("[controller]")]
public class ExpensesTypesController : ControllerBase
{
    private readonly ExpensesTypesService _expensesTypesManipulator;
    private readonly ILogger<ExpensesTypesController> _logger;
    public ExpensesTypesController(ExpensesTypesService expensesTypesManipulator, ILogger<ExpensesTypesController> logger)
    {
        _logger = logger;
        _expensesTypesManipulator = expensesTypesManipulator;
    }

    //This query returns a list of all expense types.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("TypesOfExpenses")]
    public async Task<ActionResult<List<string>>> GetAll()
    {
        _logger.LogInformation("Calling a service to execute a request to display list of types of expenses");
        var result = await _expensesTypesManipulator.InfoTypes();
        if (result.IsFailure)
        {
            _logger.LogWarning("Executing operation is fail");
            return NotFound(result.Error);
        }

        _logger.LogInformation("Executing operation is success");
        return Ok(result.Value);
    }

    //This query displays all information on the specified expense type: a list of all expenses for the selected type,
    //  the type name, the total expense amount for the type
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<TypeOfOperationsForOutputDto<Expense>>> GetByType(int id)
    {
        _logger.LogInformation($"Calling a service to execute a request to output type of expenses with {id} id");
        var result = await _expensesTypesManipulator.GetInfoOfType(id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Executing operation is fail");
            return BadRequest(result.Error);
        }
        _logger.LogInformation("Executing operation is success");
        return Ok(result.Value);
    }

    //This query returns the total amount of the expense.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("TotalSummOfExpenses")]
    public async Task<ActionResult<double>> GetTotalSummOfExpenses()
    {
        _logger.LogInformation("Calling a service to execute a request to output total sum of all expenses");
        var result = await _expensesTypesManipulator.TotalSumOfOperations();
        if (result.IsSuccess)
        {
            _logger.LogInformation("Executing operation is success");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Executing operation is fail");
            return NotFound(result.Error);
        }
    }

    //This request adds a new expense type.
    // [Authorize(Roles = "SuperUser, User")]
    [HttpPost]
    public async Task<IActionResult> AddType([FromBody] NameTypeOfOperationsDto typeOfExpenses)
    {
        _logger.LogInformation("Calling a service to execute a request of adding a new type of expenses");
        var result = await _expensesTypesManipulator.AddType(typeOfExpenses);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Execution of request is success");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Execution of request is fail");
            return BadRequest(result.Error);
        }
    }

    //This request is to update the name of the expense type, while overwriting all expense objects marked with this expense type.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] NameTypeOfOperationsDto typeOfExpenses, int id)
    {
        _logger.LogInformation("Calling a serice to execute a request to update the type of expenses");
        var result = await _expensesTypesManipulator.Update(typeOfExpenses, id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Execution of request is fail");
            return NotFound(result.Error);
        }
        else
        {
            _logger.LogInformation("Execution of request is success");
            return Ok(result.Value);
        }
    }

    //This request is to delete an expense type, which will delete all expense objects that are marked with this expense type.
    //[Authorize(Roles = "SuperUser, User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Calling a service to execute a request to delete the type of expenses and all expenses from this type");
        var result = await _expensesTypesManipulator.Delete(id);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Execution of request is success");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Execution of request is fail");
            return BadRequest(result.Error);
        }
    }
}