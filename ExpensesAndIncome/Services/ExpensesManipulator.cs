using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services;

//This service is for obtaining information on ID, deleting, or creating a new expense.

public class ExpensesManipulator
{
    public ExpensesAndIncomesDb db;

    public ExpensesManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    //Displays a list of all expenses
    public async Task<List<Expense>> InfoOfExpenses()
    {
        var list = await db.Expenses.ToListAsync();

        if (list.Count == 0)
            return null!;
        else
        {
            return list;
        }
    }
//Adds expense to existing expense categories
    public async Task AddNewExpense(ExpenseDto dto)
    {
        Expense newExpense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);

        await db.Expenses.AddAsync(newExpense);
        await db.SaveChangesAsync();
    }
//Deletes a specific expense by ID
    public async Task Delete(int id)
    {
        var Expense = await db.Expenses.FindAsync(id);
        db.Expenses.Remove(Expense!);
        await db.SaveChangesAsync();
    }
}



