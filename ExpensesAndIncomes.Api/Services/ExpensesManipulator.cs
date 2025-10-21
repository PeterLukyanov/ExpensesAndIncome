using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;
using UoW;

namespace Services;

//This service is for obtaining information on ID, deleting, or creating a new expense.

public class ExpensesManipulator
{
    private readonly IUnitOfWork _unit;
    private readonly ILogger<ExpensesManipulator> _logger;

    public ExpensesManipulator(IUnitOfWork unit, ILogger<ExpensesManipulator> logger)
    {
        _unit = unit;
        _logger = logger;
    }

    //Displays a list of all expenses
    public async Task<Result<List<Expense>>> InfoOfExpenses()
    {
        _logger.LogInformation("Executing a query to list all expenses");
        var list = await _unit.expenseRepository.GetAll().ToListAsync();

        if (list.Count == 0)
        {
            _logger.LogWarning("There are no expenses yet");
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
        _logger.LogInformation($"Executing a query to add new expense with {dto.TypeOfExpenses} type");
        var typeList = await _unit.typeOfExpensesRepository.GetAll().Select(t => t.Name).ToListAsync();
        bool typeExist = typeList.Any(t => t == dto.TypeOfExpenses);
        if (typeExist)
        {
            Expense newExpense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);

            await _unit.expenseRepository.AddAsync(newExpense);
            await _unit.SaveChangesAsync();
            return Result.Success(newExpense);
        }
        else
        {
            _logger.LogWarning($"{dto.TypeOfExpenses} type of Expenses was not found");
            return Result.Failure<Expense>("This type of Expenses was not found");
        }
    }

    //Update a specific expense by Id
    public async Task<Result<Expense>> Update(ExpenseDto dto, int Id)
    {
        _logger.LogInformation($"Executing a query to update expense by ({Id}) ID");
        var expense = await _unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.Id == Id);
        if (expense == null)
        {
            _logger.LogWarning($"There is no expense with this ({Id})ID");
            return Result.Failure<Expense>($"There is no expense with this ({Id})ID");
        }
        expense.UpdateAmount(dto.Amount);
        expense.UpdateComment(dto.Comment);
        expense.UpdateTypeOfExpenses(dto.TypeOfExpenses);

        await _unit.SaveChangesAsync();

        return Result.Success(expense);
    }

    //Deletes a specific expense by ID
    public async Task<Result<Expense>> Delete(int id)
    {
        _logger.LogInformation($"Executing a query to delete expense by ({id})ID");
        var item = await _unit.expenseRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
        if (item == null)
        {
            _logger.LogWarning($"There is no expense with this ({id})ID");
            return Result.Failure<Expense>($"There is no expense with this ({id})ID");
        }
        _unit.expenseRepository.Remove(item);
        await _unit.SaveChangesAsync();
        return Result.Success(item);
    }
}



