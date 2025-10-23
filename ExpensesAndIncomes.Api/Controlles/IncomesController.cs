using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Controllers;

//This controller processes requests: listing all incomes, adding an income, deleting an income


[ApiController]
[Route("[controller]")]
public class IncomesController : ControllerBase
{
    private readonly IncomesService _incomesManipulator;
    private readonly ILogger<IncomesController> _logger;

    public IncomesController(IncomesService incomesManipulator, ILogger<IncomesController> logger)
    {
        _incomesManipulator = incomesManipulator;
        _logger = logger;
    }

    //Request to display a list of all income items
    //[Authorize(Roles = "SuperUser, User")]
    [HttpGet("AllIncomes")]
    public async Task<ActionResult<List<Income>>> GetAll()
    {
        _logger.LogInformation("Calling a service to execute a request to display the entire list of incomes.");
        var result = await _incomesManipulator.InfoOfOperations();

        if (result.IsFailure)
        {
            _logger.LogWarning("Opretion is fail");
            return NotFound(result.Error);
        }
        _logger.LogInformation("Operation is successful");
        return Ok(result.Value);
    }

    //Request to add a new income item
    //[Authorize(Roles = "SuperUser, User")]
    [HttpPost]
    public async Task<IActionResult> AddIncome([FromBody] OperationDto dto)
    {
        _logger.LogInformation("Calling the service to perform a request to add a new income");
        var result = await _incomesManipulator.AddNewOperation(dto);
        if (result.IsSuccess)
        { 
            _logger.LogInformation("Operation is successful");
            return Ok(result.Value); 
            }
            
        _logger.LogWarning("Opretion is fail");
        return NotFound(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome([FromBody] OperationDto dto, int id)
    {
        _logger.LogInformation("Calling a service to perform a request to update an income");
        var result = await _incomesManipulator.Update(dto, id);
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

    //Request to delete a specific income by ID
    //[Authorize(Roles = "SuperUser, User")]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        _logger.LogInformation("Calling a service to perform a request to delete an income");
        var result = await _incomesManipulator.Delete(Id);
        if (result.IsFailure)
        {
            _logger.LogWarning("Operation is fail");
            return NotFound(result.Error);
        }

        _logger.LogInformation("Operation is successful");
        return Ok(result.Value);
    }

}