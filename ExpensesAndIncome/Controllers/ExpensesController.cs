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
    public ActionResult<List<Expense>> GetAll()
    {
        if (expensesManipulator.InfoOfExpenses() == null)
            return NotFound("There are no Expenses for now");
        return expensesManipulator.InfoOfExpenses();
    }

    [HttpPost]
    public IActionResult AddExpense([FromBody] ExpenseDto dto)
    {
        if (dto.Amount <= 0)
            return BadRequest("Not valid number");

        var typeOfExpenseExist = listTypesOfExpenses.listTypeOfExpenses.FirstOrDefault(c => c.NameOfType == dto.TypeOfExpenses);
        if (typeOfExpenseExist == null)
            return BadRequest("This category does not exist");

        expensesManipulator.AddNewExpense(dto);
        return Ok(dto);
    }
    [HttpDelete("{Id}")]
    public IActionResult Delete(int Id)
    {
        foreach (var listOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            foreach (var expense in listOfExpenses.listOfExpenses)
            {
                if (expense.Id == Id)
                {
                    expensesManipulator.Delete(Id);
                    return Ok(Id);
                }
            }
        }

        return NotFound($"Expense whith this Id({Id}) does not exist");
    }
}