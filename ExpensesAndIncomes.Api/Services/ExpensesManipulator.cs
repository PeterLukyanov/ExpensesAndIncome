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
    private readonly ILogger<ExpensesManipulator> logger;

    public ExpensesManipulator(IUnitOfWork _unit, ILogger<ExpensesManipulator> _logger)
    {
        unit = _unit;
        logger = _logger;
    }

    //Displays a list of all expenses
    public async Task<Result<List<Expense>>> InfoOfExpenses()
    {
        logger.LogInformation("Executing a query to list all expenses");
        var list = await unit.expenseRepository.GetAll().ToListAsync();

        if (list.Count == 0)
        {
            logger.LogWarning("There are no expenses yet");
            return Result.Failure<List<Expense>>("There are no Expenses for now");
        }
        else
        {
            return Result.Success(list);
        }
    }

    //Adds expense to existing expense categories
    public async Task<Result<Expense>> AddNewExpense(ExpenseDto dto)
    {
        logger.LogInformation($"Executing a query to add new expense with {dto.TypeOfExpenses} type");
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
        {
            logger.LogWarning($"{dto.TypeOfExpenses} type of Expenses was not found");
            return Result.Failure<Expense>("This type of Expenses was not found");
        }
    }

    //Update a specific expense by Id
    public async Task<Result<Expense>> Update(ExpenseDto dto, int Id)
    {
        logger.LogInformation($"Executing a query to update expense by ({Id}) ID");
        var expense = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.Id == Id);
        if (expense == null)
        {
            logger.LogWarning($"There is no expense with this ({Id})ID");
            return Result.Failure<Expense>($"There is no expense with this ({Id})ID");
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
        logger.LogInformation($"Executing a query to delete expense by ({id})ID");
        var item = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
        if (item == null)
        {
            logger.LogWarning($"There is no expense with this ({id})ID");
            return Result.Failure<Expense>($"There is no expense with this ({id})ID");
        }
        unit.expenseRepository.Remove(item);
        await unit.SaveChangesAsync();
        return Result.Success(item);
    }
}



