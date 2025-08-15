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

    [HttpGet("Types of Incomes")]
    public ActionResult<List<ListOfIncomes>> GetAll()
    {
        if (incomesTypeManipulator.InfoTypes() == null)
            return NotFound("There are no types of Incomes for now");
        return incomesTypeManipulator.InfoTypes();
    }
    [HttpGet("bytype/{type}")]
    public ActionResult<ListOfIncomes> GetByType(string type)
    {
        var result = incomesTypeManipulator.GetInfoOfType(type);
        if (result == null)
            return BadRequest();

        return result;
    }
    [HttpGet("Total summ of Incomes")]
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
}