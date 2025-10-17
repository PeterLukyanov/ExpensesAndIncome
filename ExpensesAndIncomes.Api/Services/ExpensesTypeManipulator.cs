using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;
using UoW;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of expenses, change a type, delete a type

namespace Services;

public class ExpensesTypesManipulator
{
    private readonly IUnitOfWork _unit;
    private readonly ILogger<ExpensesTypesManipulator> _logger;
    public ExpensesTypesManipulator(IUnitOfWork unit, ILogger<ExpensesTypesManipulator> logger)
    {
        _logger = logger;
        _unit = unit;
    }

    //Checks if there are any expenses in the program, if not, it will load the starting types
    public async Task LoadTypeOfExpenses()
    {
        _logger.LogInformation("Check if there are types of expenses");
        bool expensesList = await _unit.typeOfExpensesRepository.GetAll().AnyAsync();
        if (!expensesList)
        {
            _logger.LogInformation("Types not found, new ones being created");
            await _unit.typeOfExpensesRepository.AddAsync(new TypeOfExpenses("Food"));
            await _unit.typeOfExpensesRepository.AddAsync(new TypeOfExpenses("Relax"));
            await _unit.SaveChangesAsync();
        }
        _logger.LogInformation("Types exist");
    }

    //Displays all types of expenses that are in the program
    public async Task<Result<List<TypeOfExpenses>>> InfoTypes()
    {
        _logger.LogInformation("Executing a query to list all types");
        var typesOfExpenses = await _unit.typeOfExpensesRepository.GetAll().ToListAsync();
        if (typesOfExpenses.Count == 0)
        {
            _logger.LogWarning("Expenses types not found");
            return Result.Failure<List<TypeOfExpenses>>("There are no types of Expenses for now");
        }
        return Result.Success(typesOfExpenses);
    }

    //Displays information about the type by name, namely: the sum of all expenses by type,
    // and a list of all expense objects
    public async Task<Result<ListOfExpenses>> GetInfoOfType(int id)
    {
        _logger.LogInformation($"Executing a query to get to type of expenses with {id} id");
        var typesOfExpenses = await _unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        if (typesOfExpenses != null)
        {
            _logger.LogInformation("Type exist");
            ListOfExpenses listOfExpenses = new ListOfExpenses(typesOfExpenses.Name);
            listOfExpenses.AddTotalSummOfType(await _unit.expenseRepository.GetAll()
                .Where(e => e.TypeOfExpenses == typesOfExpenses.Name)
                .SumAsync(e => e.Amount));
            listOfExpenses.UpdateListOfExpenses(await _unit.expenseRepository.GetAll()
                .Where(e => e.TypeOfExpenses == typesOfExpenses.Name)
                .ToListAsync());
            return Result.Success(listOfExpenses);
        }
        else
        {
            _logger.LogWarning($"Type of expenses with {id} id not found");
            return Result.Failure<ListOfExpenses>("Such type of Expenses does not exist");
        }
    }

    //Displays the total amount of all expenses
    public async Task<Result<double>> TotalSumOfExpenses()
    {
        _logger.LogInformation("Executing a query to output the total sum of expenses");
        bool expensesExist = await _unit.expenseRepository.GetAll().AnyAsync();
        if (expensesExist)
        {
            return Result.Success(await _unit.expenseRepository.GetAll().Select(e => e.Amount)
                                                    .SumAsync());
        }
        else
        {
            _logger.LogWarning("There are no expenses");
            return Result.Failure<double>("There are no expenses");
        }
    }

    //Created types will be stored in a separate SQL table.
    public async Task<Result<TypeOfExpenses>> AddType(TypeOfExpensesDto typeOfExpensesDto)
    {
        _logger.LogInformation($"Executing a query to add {typeOfExpensesDto.NameOfType} type of expenses");
        var typeOfExpensesExist = await _unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfExpensesDto.NameOfType.Trim().ToLower());
        if (typeOfExpensesExist == null)
        {
            TypeOfExpenses typeOfExpenses = new TypeOfExpenses(typeOfExpensesDto.NameOfType);
            await _unit.typeOfExpensesRepository.AddAsync(typeOfExpenses);

            await _unit.SaveChangesAsync();

            return Result.Success(typeOfExpenses);
        }
        else
        {
            _logger.LogWarning($"Name {typeOfExpensesDto.NameOfType} is already exists, try another name");
            return Result.Failure<TypeOfExpenses>($"Name {typeOfExpensesDto.NameOfType} is already exists, try another name");
        }
    }

    //Updates the name of the specified type,
    // while overwriting the name of all expense objects that were marked with the current expense type
    public async Task<Result<TypeOfExpenses>> Update(TypeOfExpensesDto typeOfExpenses, int id)
    {
        _logger.LogInformation($"Executing a query to update a type of expenses with {id}id");
        var typeExist = await _unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        if (typeExist != null)
        {
            if (typeOfExpenses.NameOfType.Trim().ToLower() != typeExist.Name.ToLower())
            {
                _logger.LogInformation("Type of expenses exist");
                var listOfExpenses = await _unit.expenseRepository.GetAll().Where(e => e.TypeOfExpenses == typeExist.Name).ToListAsync();

                for (int i = 0; i < listOfExpenses.Count; i++)
                {
                    listOfExpenses[i].UpdateTypeOfExpenses(typeOfExpenses.NameOfType);
                }

                typeExist.UpdateName(typeOfExpenses.NameOfType.Trim());
                await _unit.SaveChangesAsync();
                return Result.Success(typeExist);
            }
            else
            {
                _logger.LogWarning($"Type of expenses with {typeOfExpenses.NameOfType.Trim()} name exist");
                return Result.Failure<TypeOfExpenses>($"Type of expenses with {typeOfExpenses.NameOfType.Trim()} name exist, try another name");
            }
        }
        else
        {
            _logger.LogWarning($"Type of expenses with {id} id does not exist");
            return Result.Failure<TypeOfExpenses>("Such type of Expenses does not exist");
        }
    }

    //Deletes an expense type, which also deletes all expense objects associated with that type.
    public async Task<Result<TypeOfExpenses>> Delete(int id)
    {
        _logger.LogInformation($"Executing a query to delete type of expenses with {id} id and all expenses with this type");
        var typeOfExpenses = await _unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Id == id);
        if (typeOfExpenses == null)
        {
            _logger.LogWarning($"Type of expenses with {id} id does not exist");
            return Result.Failure<TypeOfExpenses>("Such type of Expenses does not exist");
        }
        var list = await _unit.expenseRepository.GetAll().Where(c => c.TypeOfExpenses == typeOfExpenses.Name).ToListAsync();

        _unit.expenseRepository.RemoveRange(list);
        _unit.typeOfExpensesRepository.Remove(typeOfExpenses);

        await _unit.SaveChangesAsync();

        return Result.Success(typeOfExpenses);
    }
}
