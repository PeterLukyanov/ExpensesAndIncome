using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class IncomesController : ControllerBase
{
    public IncomesManipulator incomesManipulator;
    public ListTypesOfIncomes listTypesOfIncomes;


    public IncomesController(IncomesManipulator _incomesManipulator, ListTypesOfIncomes _listTypesOfIncomes)
    {
        incomesManipulator = _incomesManipulator;
        listTypesOfIncomes = _listTypesOfIncomes;
    }

    [HttpGet("AllIncomes")]
    public async Task<ActionResult<List<Income>>> GetAll()
    {
        var incomes = await incomesManipulator.InfoOfIncomes();

        if (incomes == null||incomes.Count==0)
            return NotFound("There are no Incomes for now");

        return incomes;
    }
    [HttpPost]
    public async Task<IActionResult> AddIncome([FromBody] IncomeDto dto)
    {
      

        var typeOfIncome = listTypesOfIncomes.ListTypeOfIncomes.FirstOrDefault(c => c.NameOfType == dto.TypeOfIncomes);
        if (typeOfIncome == null)
            return BadRequest("This category does not exist");

        await incomesManipulator.AddNewIncome(dto);
        return Ok(dto);
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        foreach (var listOfIncomes in listTypesOfIncomes.ListTypeOfIncomes)
        {
            foreach (var income in listOfIncomes.listOfIncomes)
            {
                if (income.Id == Id)
                {
                    await incomesManipulator.Delete(Id);
                    return Ok(Id);
                }
            }
        }

        return NotFound($"Income whith this Id({Id}) does not exist");
    }

}