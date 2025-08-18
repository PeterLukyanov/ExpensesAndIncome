using Models;
using Services;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class IncomesTypeController : ControllerBase
{
    private readonly IncomesTypeManipulator incomesTypeManipulator;
    public ListTypesOfIncomes listTypesOfIncomes;
    public IncomesTypeController(IncomesTypeManipulator _incomesTypeManipulator, ListTypesOfIncomes _listTypesOfIncomes)
    {
        incomesTypeManipulator = _incomesTypeManipulator;
        listTypesOfIncomes = _listTypesOfIncomes;
    }

    [HttpGet("TypesOfIncomes")]
    public ActionResult<List<ListOfIncomes>> GetAll()
    {
        if (incomesTypeManipulator.InfoTypes() == null)
            return NotFound("There are no types of Incomes for now");
        return incomesTypeManipulator.InfoTypes();
    }
    [HttpGet("{type}")]
    public ActionResult<ListOfIncomes> GetByType(string type)
    {
        var result = incomesTypeManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest();

        return result;
    }
    [HttpGet("TotalSummOfIncomes")]
    public double GetTotalSummOfIncomes()
    {
        return incomesTypeManipulator.TotalSummOfIncomes();
    }
    [HttpPost]
    public IActionResult AddType([FromBody] ListOfIncomes listOfIncomes)
    {
        var typeOfIncome = listTypesOfIncomes.listTypeOfIncomes.FirstOrDefault(c => c.NameOfType.ToLower() == listOfIncomes.NameOfType.Trim().ToLower());
        if (typeOfIncome == null)
        {
            incomesTypeManipulator.AddType(listOfIncomes);
            return Ok(listOfIncomes);
        }
        return BadRequest($"Name {listOfIncomes.NameOfType} is already exists, try another name");

    }
    [HttpPut("{nameOfType}")]
    public IActionResult Update(string nameOfType, [FromBody] ListOfIncomes listOfIncomes)
    {
        var existingType = incomesTypeManipulator.GetInfoOfType(listOfIncomes.NameOfType);
        if (existingType is null)
            return NotFound();

        incomesTypeManipulator.Update(listOfIncomes, nameOfType);

        return NoContent();
    }
    [HttpDelete("{nameOfType}")]
    public IActionResult Delete(string nameOfType)
    {
        incomesTypeManipulator.Delete(nameOfType);

        if (listTypesOfIncomes.listTypeOfIncomes.FirstOrDefault(c => c.NameOfType == nameOfType) == null)
            return Ok(nameOfType);
        else
            return StatusCode(500, "Something goes wrong");
    }
}