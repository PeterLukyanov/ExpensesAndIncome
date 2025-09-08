using Dtos;
using Models;
using Db;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using UoW;

namespace Services;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of incomes, change a type, delete a type

public class IncomesTypeManipulator
{
    private readonly IUnitOfWork unit;
    private readonly ILogger<IncomesTypeManipulator> logger;
    public IncomesTypeManipulator(IUnitOfWork _unit, ILogger<IncomesTypeManipulator> _logger)
    {
        logger = _logger;
        unit = _unit;
    }

    //Checks if there are any incomes in the program, if not, it will load the starting types
    public async Task LoadTypeOfIncomes()
    {
        logger.LogInformation("Check if there are types of incomes");
        bool incomesExist = await unit.typeOfIncomesRepository.GetAll().AnyAsync();
        if (!incomesExist)
        {
            logger.LogInformation("Types not found, new ones being created");
            await unit.typeOfIncomesRepository.AddAsync(new TypeOfIncomes("Salary"));
            await unit.typeOfIncomesRepository.AddAsync(new TypeOfIncomes("Other"));
            await unit.SaveChangesAsync();
        }
    }
    //Displays all types of incomes that are in the program
    public async Task<Result<List<TypeOfIncomes>>> InfoTypes()
    {
        logger.LogInformation("Executing a query to list all types");
        var typesOfIncomes = await unit.typeOfIncomesRepository.GetAll().ToListAsync();
        if (typesOfIncomes.Count == 0)
        {
            logger.LogWarning("Incomes types not found");
            return Result.Failure<List<TypeOfIncomes>>("There are no types of Incomes for now");
        }
        return Result.Success(typesOfIncomes);
    }

    //Displays information about the type by name, namely: the sum of all incomes by type,
    // and a list of all income objects
    public async Task<Result<ListOfIncomes>> GetInfoOfType(string type)
    {
        logger.LogInformation($"Executing a query to get {type} type of incomes");
        var typesOfIncomes = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == type);

        if (typesOfIncomes != null)
        {
            ListOfIncomes listOfIncomes = new ListOfIncomes(type);
            listOfIncomes.AddTotalSummOfType(await unit.incomeRepository.GetAll()
                .Where(i => i.TypeOfIncomes == type)
                .SumAsync(e => e.Amount));
            listOfIncomes.UpdateListOfIncomes(await unit.incomeRepository.GetAll()
                .Where(i => i.TypeOfIncomes == type)
                .ToListAsync());
            return Result.Success(listOfIncomes);
        }
        else
        {
            logger.LogWarning($"Type with {type} name not found");
            return Result.Failure<ListOfIncomes>("Such type of Incomes does not exist");
        }
    }

    //Displays the total amount of all incomes
    public async Task<Result<double>> TotalSummOfIncomes()
    {
        logger.LogInformation("Executing a query to output the total sum of incomes");
        bool incomesExist = await unit.incomeRepository.GetAll().AnyAsync();
        if (incomesExist)
        {
            return Result.Success(await unit.incomeRepository.GetAll().Select(e => e.Amount)
                                        .SumAsync());
        }
        else
        {
            logger.LogWarning("There are no incomes");
            return Result.Failure<double>("There are no incomes");
        }
    }

    //The created types will be stored in a separate SQL table.
    public async Task<Result<TypeOfIncomes>> AddType(TypeOfIncomesDto typeOfIncomesDto)
    {
        logger.LogInformation($"Executing a query to add {typeOfIncomesDto.NameOfType} type of incomes");
        var typeOfIncomesExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfIncomesDto.NameOfType.Trim().ToLower());
        if (typeOfIncomesExist == null)
        {
            TypeOfIncomes typeOfIncomes = new TypeOfIncomes(typeOfIncomesDto.NameOfType);
            await unit.typeOfIncomesRepository.AddAsync(typeOfIncomes);

            await unit.SaveChangesAsync();

            return Result.Success(typeOfIncomes);
        }
        else
        {
            logger.LogWarning($"Name {typeOfIncomesDto.NameOfType} is already exist, try another name");
            return Result.Failure<TypeOfIncomes>($"Name {typeOfIncomesDto.NameOfType} is already exists, try another name");
        }
    }

    //Updates the name of the specified type,
    // while overwriting the name of all income objects that were marked with the current income type
    public async Task<Result<TypeOfIncomes>> Update(TypeOfIncomesDto typeOfIncomes, string nameOfType)
    {
        logger.LogInformation($"Executing a query to update a {nameOfType} type of incomes");
        var typeExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeExist != null)
        {
            var listOfIncomes = await unit.incomeRepository.GetAll().Where(e => e.TypeOfIncomes == nameOfType).ToListAsync();

            for (int i = 0; i < listOfIncomes.Count; i++)
            {
                listOfIncomes[i].UpdateTypeOfIncomes(typeOfIncomes.NameOfType);
            }

            typeExist.UpdateName(nameOfType);
            await unit.SaveChangesAsync();
            return Result.Success(typeExist);
        }
        else
        {
            logger.LogWarning($"{nameOfType} type of incomes does not exist");
            return Result.Failure<TypeOfIncomes>("Such type of Incomes does not exist");
        }
    }
    //Deletes an income type, which also deletes all income objects associated with that type.

    public async Task<Result<TypeOfIncomes>> Delete(string nameOfType)
    {
        logger.LogInformation($"Executing a query to delete {nameOfType} type of incomes and all incomes with this type");
        var typeOfIncomes = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeOfIncomes == null)
        {
            logger.LogWarning($"{nameOfType} type of incomes does not exist");
            return Result.Failure<TypeOfIncomes>("Such type of Incomes does not exist");
        }
        var list = await unit.incomeRepository.GetAll().Where(c => c.TypeOfIncomes == nameOfType).ToListAsync();

        unit.incomeRepository.RemoveRange(list);
        unit.typeOfIncomesRepository.Remove(typeOfIncomes);

        await unit.SaveChangesAsync();

        return Result.Success(typeOfIncomes);
    }
}