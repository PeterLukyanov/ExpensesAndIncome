using Models;
using Services;
using Microsoft.AspNetCore.Mvc;
using Dtos;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    public ExpensesManipulator expensesManipulator;
    public ListTypesOfExpenses listTypesOfExpenses;

    public ExpensesController(ExpensesManipulator _expensesManipulator, ListTypesOfExpenses _listTypesOfExpenses)
    {
        expensesManipulator = _expensesManipulator;
        listTypesOfExpenses = _listTypesOfExpenses;
    }

    [HttpGet("AllExpenses")]
    public async Task<ActionResult<List<Expense>>> GetAll()
    {

        var expenses = await expensesManipulator.InfoOfExpenses();

        if (expenses == null || expenses.Count == 0)
            return NotFound("There are no Expenses for now");

        return expenses;
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto dto)
    {
        var typeOfExpenseExist = listTypesOfExpenses.ListTypeOfExpenses.FirstOrDefault(c => c.NameOfType == dto.TypeOfExpenses);
        if (typeOfExpenseExist == null)
            return BadRequest("This category does not exist");

        await expensesManipulator.AddNewExpense(dto);
        return Ok(dto);
    }
    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete(int Id)
    {
        foreach (var listOfExpenses in listTypesOfExpenses.ListTypeOfExpenses)
        {
            foreach (var expense in listOfExpenses.listOfExpenses)
            {
                if (expense.Id == Id)
                {
                    await expensesManipulator.Delete(Id);
                    return Ok(Id);
                }
            }
        }

        return NotFound($"Expense whith this Id({Id}) does not exist");
    }
}