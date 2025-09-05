using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;
using UoW;

namespace Services;

//This service is for obtaining information on ID, deleting, or creating a new expense.

public class ExpensesManipulator
{
    private readonly IUnitOfWork unit;

    public ExpensesManipulator(IUnitOfWork _unit)
    {
        unit = _unit;
    }

    //Displays a list of all expenses
    public async Task<Result<List<Expense>>> InfoOfExpenses()
    {
        var list = await unit.expenseRepository.GetAll().ToListAsync();

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
        var typeList = await unit.typeOfExpensesRepository.GetAll().Select(t => t.Name).ToListAsync();
        bool typeExist = typeList.Any(t => t == dto.TypeOfExpenses);
        if (typeExist)
        {
            Expense newExpense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);

            await unit.expenseRepository.AddAsync(newExpense);
            await unit.SaveChangesAsync();
            return Result.Success(newExpense);
        }
        else
            return Result.Failure<Expense>("This type of Expenses does not found");
    }

    //Update a specific expense by Id
    public async Task<Result<Expense>> Update(ExpenseDto dto, int Id)
    {
        var expense = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.Id == Id);
        if (expense == null)
        {
            return Result.Failure<Expense>($"Expense whith this Id({Id}) does not exist");
        }
        expense.UpdateAmount(dto.Amount);
        expense.UpdateComment(dto.Comment);
        expense.UpdateTypeOfExpenses(dto.TypeOfExpenses);

        await unit.SaveChangesAsync();

        return Result.Success(expense);
    }

    //Deletes a specific expense by ID
    public async Task<Result<Expense>> Delete(int id)
    {
        var item = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
        if (item == null)
        {
            return Result.Failure<Expense>($"Expense whith this Id({id}) does not exist");
        }
        unit.expenseRepository.Remove(item);
        await unit.SaveChangesAsync();
        return Result.Success(item);
    }
}



