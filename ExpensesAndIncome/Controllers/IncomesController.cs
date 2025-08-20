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
        if (incomesManipulator.InfoOfIncomes() == null)
            return NotFound("There are no Incomes for now");
        return await incomesManipulator.InfoOfIncomes();
    }
    [HttpPost]
    public IActionResult AddIncome([FromBody] IncomeDto dto)
    {
        if (dto.Amount <= 0)
            return BadRequest("Not valid number");

        var typeOfIncome = listTypesOfIncomes.ListTypeOfIncomes.FirstOrDefault(c => c.NameOfType == dto.TypeOfIncomes);
        if (typeOfIncome == null)
            return BadRequest("This category does not exist");

        incomesManipulator.AddNewIncome(dto);
        return Ok(dto);
    }
    [HttpDelete("{Id}")]
    public IActionResult Delete(int Id)
    {
        foreach (var listOfIncomes in listTypesOfIncomes.ListTypeOfIncomes)
        {
            foreach (var income in listOfIncomes.listOfIncomes)
            {
                if (income.Id == Id)
                {
                    incomesManipulator.Delete(Id);
                    return Ok(Id);
                }
            }
        }

        return NotFound($"Income whith this Id({Id}) does not exist");
    }

}