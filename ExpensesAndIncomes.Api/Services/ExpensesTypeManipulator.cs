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
    private readonly IUnitOfWork unit;
    private readonly ILogger<ExpensesTypesManipulator> logger;
    public ExpensesTypesManipulator(IUnitOfWork _unit, ILogger<ExpensesTypesManipulator> _logger)
    {
        logger = _logger;
        unit = _unit;
    }

    //Checks if there are any expenses in the program, if not, it will load the starting types
    public async Task LoadTypeOfExpenses()
    {
        logger.LogInformation("Check if there are types of expenses");
        bool expensesList = await unit.typeOfExpensesRepository.GetAll().AnyAsync();
        if (!expensesList)
        {
            logger.LogInformation("Types not found, new ones being created");
            await unit.typeOfExpensesRepository.AddAsync(new TypeOfExpenses("Food"));
            await unit.typeOfExpensesRepository.AddAsync(new TypeOfExpenses("Relax"));
            await unit.SaveChangesAsync();
        }
        logger.LogInformation("Types exist");
    }

    //Displays all types of expenses that are in the program
    public async Task<Result<List<TypeOfExpenses>>> InfoTypes()
    {
        logger.LogInformation("Executing a query to list all types");
        var typesOfExpenses = await unit.typeOfExpensesRepository.GetAll().ToListAsync();
        if (typesOfExpenses.Count == 0)
        {
            logger.LogWarning("Expenses types not found");
            return Result.Failure<List<TypeOfExpenses>>("There are no types of Expenses for now");
        }
        return Result.Success(typesOfExpenses);
    }

    //Displays information about the type by name, namely: the sum of all expenses by type,
    // and a list of all expense objects
    public async Task<Result<ListOfExpenses>> GetInfoOfType(string type)
    {
        logger.LogInformation($"Executing a query to get {type} type of expenses");
        var typesOfExpenses = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == type);
        if (typesOfExpenses != null)
        {
            ListOfExpenses listOfExpenses = new ListOfExpenses(type);
            listOfExpenses.AddTotalSummOfType(await unit.expenseRepository.GetAll()
                .Where(e => e.TypeOfExpenses == type)
                .SumAsync(e => e.Amount));
            listOfExpenses.UpdateListOfExpenses(await unit.expenseRepository.GetAll()
                .Where(e => e.TypeOfExpenses == type)
                .ToListAsync());
            return Result.Success(listOfExpenses);
        }
        else
        {
            logger.LogWarning($"Type with {type} name not found");
            return Result.Failure<ListOfExpenses>("Such type of Expenses does not exist");
        }
    }

    //Displays the total amount of all expenses
    public async Task<Result<double>> TotalSumOfExpenses()
    {
        logger.LogInformation("Executing a query to output the total sum of expenses");
        bool expensesExist = await unit.expenseRepository.GetAll().AnyAsync();
        if (expensesExist)
        {
            return Result.Success(await unit.expenseRepository.GetAll().Select(e => e.Amount)
                                                    .SumAsync());
        }
        else
        {
            logger.LogWarning("There are no expenses");
            return Result.Failure<double>("There are no expenses");
        }
    }

    //Created types will be stored in a separate SQL table.
    public async Task<Result<TypeOfExpenses>> AddType(TypeOfExpensesDto typeOfExpensesDto)
    {
        logger.LogInformation($"Executing a query to add {typeOfExpensesDto.NameOfType} type of expenses");
        var typeOfExpensesExist = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfExpensesDto.NameOfType.Trim().ToLower());
        if (typeOfExpensesExist == null)
        {
            TypeOfExpenses typeOfExpenses = new TypeOfExpenses(typeOfExpensesDto.NameOfType);
            await unit.typeOfExpensesRepository.AddAsync(typeOfExpenses);

            await unit.SaveChangesAsync();

            return Result.Success(typeOfExpenses);
        }
        else
        {
            logger.LogWarning($"Name {typeOfExpensesDto.NameOfType} is already exists, try another name");
            return Result.Failure<TypeOfExpenses>($"Name {typeOfExpensesDto.NameOfType} is already exists, try another name");
        }
    }

    //Updates the name of the specified type,
    // while overwriting the name of all expense objects that were marked with the current expense type
    public async Task<Result<TypeOfExpenses>> Update(TypeOfExpensesDto typeOfExpenses, string nameOfType)
    {
        logger.LogInformation($"Executing a query to update a {nameOfType} type of expenses");
        var typeExist = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeExist != null)
        {
            var listOfExpenses = await unit.expenseRepository.GetAll().Where(e => e.TypeOfExpenses == nameOfType).ToListAsync();

            for (int i = 0; i < listOfExpenses.Count; i++)
            {
                listOfExpenses[i].UpdateTypeOfExpenses(typeOfExpenses.NameOfType);
            }

            typeExist.UpdateName(nameOfType);
            await unit.SaveChangesAsync();
            return Result.Success(typeExist);
        }
        else
        {
            logger.LogWarning($"{nameOfType} type of expenses does not exist");
            return Result.Failure<TypeOfExpenses>("Such type of Expenses does not exist");
        }
    }

    //Deletes an expense type, which also deletes all expense objects associated with that type.
    public async Task<Result<TypeOfExpenses>> Delete(string nameOfType)
    {
        logger.LogInformation($"Executing a query to delete {nameOfType} type of expenses and all expenses with this type");
        var typeOfExpenses = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeOfExpenses == null)
        {
            logger.LogWarning($"{nameOfType} type of expenses does not exist");
            return Result.Failure<TypeOfExpenses>("Such type of Expenses does not exist");
        }
        var list = await unit.expenseRepository.GetAll().Where(c => c.TypeOfExpenses == nameOfType).ToListAsync();

        unit.expenseRepository.RemoveRange(list);
        unit.typeOfExpensesRepository.Remove(typeOfExpenses);

        await unit.SaveChangesAsync();

        return Result.Success(typeOfExpenses);
    }
}
