using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

//This controller processes requests: listing all expenses, adding an expense, deleting an expense

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ExpensesService _expensesManipulator;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(ExpensesService expensesManipulator, ILogger<ExpensesController> logger)
    {
        _expensesManipulator = expensesManipulator;
        _logger = logger;
    }

    //Request to display a list of all expense items
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("AllExpenses")]
    public async Task<ActionResult<List<Expense>>> GetAll()
    {
        _logger.LogInformation("Calling a service to execute a request to display the entire list of expenses.");
        var result = await _expensesManipulator.InfoOfOperations();
        if (result.IsSuccess)
            {
            _logger.LogInformation("Operation is successful");
            return Ok(result.Value);}
        else
        {
            _logger.LogWarning("Opretion is fail");
            return NotFound(result.Error);
        }
    }

    //Request to add a new expense item
    //[Authorize(Roles = "SuperUser, User")]
    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] OperationDto dto)
    {
        _logger.LogInformation("Calling the service to perform a request to add a new expense");
        var result = await _expensesManipulator.AddNewOperation(dto);
        if (result.IsSuccess)
        {
            _logger.LogInformation("Operation is successful");
            return Ok(result.Value);
        }
        else
        {
            _logger.LogWarning("Opretion is fail");
            return NotFound(result.Error);
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense([FromBody] OperationDto dto, int id)
    {
        _logger.LogInformation("Calling a service to perform a request to update an expense");
        var result = await _expensesManipulator.Update(dto, id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Operation is fail");
            return NotFound(result.Error);
        }
        else
        {
            _logger.LogInformation("Operation is successful");
            return Ok(result.Value);
        }
    }

    //Request to delete a specific expense by ID
    //[Authorize(Roles = "SuperUser, User")]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        _logger.LogInformation("Calling a service to perform a request to delete an expense");
        var result = await _expensesManipulator.Delete(Id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Operation is fail");
            return NotFound(result.Error);
        }

        _logger.LogInformation("Operation is successful");
        return Ok(result.Value);
    }
}