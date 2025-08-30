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
    public ExpensesAndIncomesDb db;
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
        if (result.Count == 0)
            return NotFound("There are no types of Incomes for now");

        return Ok(result);
    }
    //This query displays all information on the specified income type: a list of all incomes for the selected type,
    //  the type name, the total income amount for the type
    [HttpGet("{type}")]
    public async Task<ActionResult<ListOfIncomes>> GetByType(string type)
    {
        var result = await incomesTypesManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest("Such type of Expenses does not exist");

        return result;
    }
    //This query returns the total amount of the income.
    [HttpGet("TotalSummOfIncomes")]
    public async Task<double> GetTotalSummOfIncomes()
    {
        return await incomesTypesManipulator.TotalSummOfIncomes();
    }
    //This request adds a new income type.
    [HttpPost]
    public async Task<IActionResult> AddType([FromBody] TypeOfIncomesDto listOfIncomes)
    {
        var typesOfIncomes = await db.TypesOfIncomes.Select(t => t.Name).ToListAsync();
        var typeExist = typesOfIncomes.FirstOrDefault(c => c.ToLower() == listOfIncomes.NameOfType.Trim().ToLower());
        if (typeExist == null)
        {
            await incomesTypesManipulator.AddType(listOfIncomes);
            return Ok(listOfIncomes);
        }
        else
            return BadRequest($"Name {listOfIncomes.NameOfType} is already exists, try another name");

    }
    //This request is to update the name of the income type, while overwriting all income objects marked with this income type.
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update([FromBody] TypeOfIncomesDto listOfIncomes, string nameOfType)
    {
        var existingType = await incomesTypesManipulator.GetInfoOfType(listOfIncomes.NameOfType);
        if (existingType is null)
            return NotFound("Such type of Incomes does not exist");

        await incomesTypesManipulator.Update(listOfIncomes, nameOfType);

        return Ok(nameOfType);
    }
    //This request is to delete an income type, which will delete all income objects that are marked with this income type.
    [HttpDelete("{nameOfType}")]
    public async Task<IActionResult> Delete(string nameOfType)
    {
        var listOfTypes = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        if (listOfTypes.FirstOrDefault(c => c == nameOfType) != null)
        {
            await incomesTypesManipulator.Delete(nameOfType);
            return Ok(nameOfType);
        }
        else
            return BadRequest("Such type of Incomes does not exist");
    }
}