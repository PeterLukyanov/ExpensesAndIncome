using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;
using Db;
using Microsoft.EntityFrameworkCore;

namespace Controllers;

//This controller processes requests: listing all expenses, adding an expense, deleting an expense

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    public ExpensesManipulator expensesManipulator;
    public ExpensesAndIncomesDb db;

    public ExpensesController(ExpensesManipulator _expensesManipulator, ExpensesAndIncomesDb _db)
    {
        expensesManipulator = _expensesManipulator;
        db = _db;
    }
    //Request to display a list of all expense items
    [HttpGet("AllExpenses")]
    public async Task<ActionResult<List<Expense>>> GetAll()
    {

        var expenses = await expensesManipulator.InfoOfExpenses();

        if (expenses == null || expenses.Count == 0)
            return NotFound("There are no Expenses for now");

        return expenses;
    }
    //Request to add a new expense item
    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto dto)
    {
        var typeList = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        bool typeExist = typeList.Any(t => t == dto.TypeOfExpenses);
        if (typeExist)
        {
            await expensesManipulator.AddNewExpense(dto);
            return Ok(dto);
        }
        return BadRequest("This type of Expenses does not found");
    }
    //Request to delete a specific expense by ID
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        var item = await db.Expenses.FirstOrDefaultAsync(c => c.Id == Id);
        if (item == null)
        {
            return NotFound($"Expense whith this Id({Id}) does not exist");
        }
        await expensesManipulator.Delete(Id);
        return Ok(Id);
    }
}