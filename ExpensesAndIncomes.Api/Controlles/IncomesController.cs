using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

//This controller processes requests: listing all incomes, adding an income, deleting an income


[ApiController]
[Route("[controller]")]
public class IncomesController : ControllerBase
{
    public IncomesManipulator incomesManipulator;
    public ExpensesAndIncomesDb db;

    public IncomesController(IncomesManipulator _incomesManipulator, ExpensesAndIncomesDb _db)
    {
        incomesManipulator = _incomesManipulator;
        db = _db;
    }
//Request to display a list of all income items
    [HttpGet("AllIncomes")]
    public async Task<ActionResult<List<Income>>> GetAll()
    {
        var incomes = await incomesManipulator.InfoOfIncomes();

        if (incomes == null||incomes.Count==0)
            return NotFound("There are no Incomes for now");

        return incomes;
    }
    //Request to add a new income item
    [HttpPost]
    public async Task<IActionResult> AddIncome([FromBody] IncomeDto dto)
    {
       var typeList = await db.TypesOfIncomes.Select(t => t.Name).ToListAsync();
        bool typeExist = typeList.Any(t => t == dto.TypeOfIncomes);
        if (typeExist)
        {
            await incomesManipulator.AddNewIncome(dto);
            return Ok(dto);
        }
        return BadRequest("This type of Incomes does not found");
    }
    //Request to delete a specific income by ID
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
         var item = await db.Incomes.FirstOrDefaultAsync(c=>c.Id==Id);
        if (item == null)
        { 
            return NotFound($"Income whith this Id({Id}) does not exist");
        }
        await incomesManipulator.Delete(Id);
            return Ok(Id);  
    }

}