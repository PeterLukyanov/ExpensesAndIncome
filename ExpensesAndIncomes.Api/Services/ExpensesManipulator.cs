using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;

namespace Services;

//This service is for obtaining information on ID, deleting, or creating a new expense.

public class ExpensesManipulator
{
    private readonly ExpensesAndIncomesDb db;

    public ExpensesManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }

    //Displays a list of all expenses
    public async Task<Result<List<Expense>>> InfoOfExpenses()
    {
        var list = await db.Expenses.ToListAsync();

        if (list.Count == 0)
            return Result.Failure<List<Expense>>("There are no Expenses for now");
        else
        {
            return Result.Success(list);
        }
    }

    //Adds expense to existing expense categories
    public async Task<Result<Expense>> AddNewExpense(ExpenseDto dto)
    {
        var typeList = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        bool typeExist = typeList.Any(t => t == dto.TypeOfExpenses);
        if (typeExist)
        {
            Expense newExpense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);

            await db.Expenses.AddAsync(newExpense);
            await db.SaveChangesAsync();
            return Result.Success(newExpense);
        }
        else
            return Result.Failure<Expense>("This type of Expenses does not found");
    }

    //Update a specific expense by Id
    public async Task<Result<Expense>> Update(ExpenseDto dto, int Id)
    {
        var expense = await db.Expenses.FirstOrDefaultAsync(e => e.Id == Id);
        if (expense == null)
        {
            return Result.Failure<Expense>($"Expense whith this Id({Id}) does not exist");
        }
        expense.UpdateAmount(dto.Amount);
        expense.UpdateComment(dto.Comment);
        expense.UpdateTypeOfExpenses(dto.TypeOfExpenses);

        await db.SaveChangesAsync();

        return Result.Success(expense);
    }

    //Deletes a specific expense by ID
    public async Task<Result<Expense>> Delete(int id)
    {
        var item = await db.Expenses.FirstOrDefaultAsync(c => c.Id == id);
        if (item == null)
        {
            return Result.Failure<Expense>($"Expense whith this Id({id}) does not exist");
        }
        db.Expenses.Remove(item);
        await db.SaveChangesAsync();
        return Result.Success(item);
    }
}



