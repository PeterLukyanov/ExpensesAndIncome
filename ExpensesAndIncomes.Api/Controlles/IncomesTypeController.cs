using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

//This controller processes requests: show all income types,
// show all information about a specific type, display the total amount of incomes,
// add a new income type, update an income type, delete an income type


[ApiController]
[Route("[controller]")]
public class IncomesTypeController : ControllerBase
{
    private readonly IncomesTypeManipulator incomesTypesManipulator;
    private readonly ExpensesAndIncomesDb db;
    public IncomesTypeController(IncomesTypeManipulator _incomesTypeManipulator, ExpensesAndIncomesDb _db)
    {
        incomesTypesManipulator = _incomesTypeManipulator;
        db = _db;
    }

    //This query returns a list of all income types.
    [HttpGet("TypesOfIncomes")]
    public async Task<ActionResult<List<string>>> GetAll()
    {
        var result = await incomesTypesManipulator.InfoTypes();
        if (result.IsFailure)
            return NotFound(result.Error);
        return Ok(result.Value);
    }

    //This query displays all information on the specified income type: a list of all incomes for the selected type,
    //  the type name, the total income amount for the type
    [HttpGet("{type}")]
    public async Task<ActionResult<ListOfIncomes>> GetByType(string type)
    {
        var result = await incomesTypesManipulator.GetInfoOfType(type);
        if (result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }

    //This query returns the total amount of the income.
    [HttpGet("TotalSummOfIncomes")]
    public async Task<ActionResult<double>> GetTotalSummOfIncomes()
    {
        var result = await incomesTypesManipulator.TotalSummOfIncomes();
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return NotFound(result.Error);
    }

    //This request adds a new income type.
    [HttpPost]
    public async Task<IActionResult> AddType([FromBody] TypeOfIncomesDto typeOfIncomes)
    {
        var result = await incomesTypesManipulator.AddType(typeOfIncomes);
        if (result.IsSuccess)  
            return Ok(result.Value);
        else
            return BadRequest(result.Error);
    }

    //This request is to update the name of the income type, while overwriting all income objects marked with this income type.
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update([FromBody] TypeOfIncomesDto listOfIncomes, string nameOfType)
    {
        var result = await incomesTypesManipulator.GetInfoOfType(listOfIncomes.NameOfType);
        if (result.IsFailure)
            return NotFound(result.Error);
        else
            return Ok(result.Value);
    }

    //This request is to delete an income type, which will delete all income objects that are marked with this income type.
    [HttpDelete("{nameOfType}")]
    public async Task<IActionResult> Delete(string nameOfType)
    {
        var result = await incomesTypesManipulator.Delete(nameOfType);
        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return BadRequest(result.Error);
    }
}