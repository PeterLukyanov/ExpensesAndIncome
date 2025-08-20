using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class IncomesTypeController : ControllerBase
{
    private readonly IncomesTypeManipulator incomesTypesManipulator;
    public ListTypesOfIncomes listTypesOfIncomes;
    public IncomesTypeController(IncomesTypeManipulator _incomesTypeManipulator, ListTypesOfIncomes _listTypesOfIncomes)
    {
        incomesTypesManipulator = _incomesTypeManipulator;
        listTypesOfIncomes = _listTypesOfIncomes;
    }

    [HttpGet("TypesOfIncomes")]
    public ActionResult<List<ListOfIncomes>> GetAll()
    {
        var result = incomesTypesManipulator.InfoTypes();
        if (result == null)
            return NotFound("There are no types of Incomes for now");

        return result;
    }
    [HttpGet("{type}")]
    public ActionResult<ListOfIncomes> GetByType(string type)
    {
        var result = incomesTypesManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest("Such type of Expenses does not exist");

        return result;
    }
    [HttpGet("TotalSummOfIncomes")]
    public double GetTotalSummOfIncomes()
    {
        return incomesTypesManipulator.TotalSummOfIncomes();
    }
    [HttpPost]
    public async Task <IActionResult> AddType([FromBody] TypeOfIncomesDto listOfIncomes)
    {
        var typeOfIncome = listTypesOfIncomes.ListTypeOfIncomes.FirstOrDefault(c => c.NameOfType.ToLower() == listOfIncomes.NameOfType.Trim().ToLower());
        if (typeOfIncome == null)
        {
            await incomesTypesManipulator.AddType(listOfIncomes);
            return Ok(listOfIncomes);
        }
        return BadRequest($"Name {listOfIncomes.NameOfType} is already exists, try another name");

    }
    [HttpPut("{nameOfType}")]
    public async Task<IActionResult> Update(string nameOfType, [FromBody] TypeOfIncomesDto listOfIncomes)
    {
        var existingType = incomesTypesManipulator.GetInfoOfType(listOfIncomes.NameOfType);
        if (existingType is null)
            return NotFound();

        await incomesTypesManipulator.Update(listOfIncomes, nameOfType);

        return Ok(nameOfType);
    }
    [HttpDelete("{nameOfType}")]
    public async Task <IActionResult> Delete(string nameOfType)
    {


        if (listTypesOfIncomes.ListTypeOfIncomes.FirstOrDefault(c => c.NameOfType == nameOfType) != null)
        {
            await incomesTypesManipulator.Delete(nameOfType);
            return Ok(nameOfType);
        }
        else
            return BadRequest("Such type of Incomes does not exist");
    }
}