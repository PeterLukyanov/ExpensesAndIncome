using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;

namespace Controllers;

//This controller processes requests: listing all incomes, adding an income, deleting an income


[ApiController]
[Route("[controller]")]
public class IncomesController : ControllerBase
{
    private readonly IncomesManipulator incomesManipulator;

    public IncomesController(IncomesManipulator _incomesManipulator)
    {
        incomesManipulator = _incomesManipulator;
    }

    //Request to display a list of all income items
    [HttpGet("AllIncomes")]
    public async Task<ActionResult<List<Income>>> GetAll()
    {
        var result = await incomesManipulator.InfoOfIncomes();

        if (result.IsFailure)
            return NotFound(result.Error);
        return Ok(result.Value);
    }

    //Request to add a new income item
    [HttpPost]
    public async Task<IActionResult> AddIncome([FromBody] IncomeDto dto)
    {
        var result = await incomesManipulator.AddNewIncome(dto);
        if (result.IsSuccess)
            return Ok(result.Value);
        return NotFound(result.Error);
    }

    //Request to delete a specific income by ID
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var result = await incomesManipulator.Delete(Id);
        if (result.IsFailure)
            return NotFound(result.Error);
        return Ok(result.Value);
    }

}