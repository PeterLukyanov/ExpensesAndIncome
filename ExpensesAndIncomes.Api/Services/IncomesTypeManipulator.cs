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
    public IncomesTypeManipulator(IUnitOfWork _unit)
    {
        unit = _unit;
    }

    //Checks if there are any incomes in the program, if not, it will load the starting types
    public async Task LoadTypeOfIncomes()
    {
        bool incomesExist = await unit.typeOfIncomesRepository.GetAll().AnyAsync();
        if (!incomesExist)
        {
            await unit.typeOfIncomesRepository.AddAsync(new TypeOfIncomes("Salary"));
            await unit.typeOfIncomesRepository.AddAsync(new TypeOfIncomes("Other"));
            await unit.SaveChangesAsync();
        }
    }
    //Displays all types of incomes that are in the program
    public async Task<Result<List<TypeOfIncomes>>> InfoTypes()
    {
        var typesOfIncomes = await unit.typeOfIncomesRepository.GetAll().ToListAsync();
        if (typesOfIncomes.Count == 0)
        {
            return Result.Failure<List<TypeOfIncomes>>("There are no types of Incomes for now");
        }
        return Result.Success(typesOfIncomes);
    }

    //Displays information about the type by name, namely: the sum of all incomes by type,
    // and a list of all income objects
    public async Task<Result<ListOfIncomes>> GetInfoOfType(string type)
    {
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
        return Result.Failure<ListOfIncomes>("Such type of Incomes does not exist");
    }

    //Displays the total amount of all incomes
    public async Task<Result<double>> TotalSummOfIncomes()
    {
        bool incomesExist = await unit.incomeRepository.GetAll().AnyAsync();
        if (incomesExist)
        {
            return Result.Success(await unit.incomeRepository.GetAll().Select(e => e.Amount)
                                        .SumAsync());
        }
        else
            return Result.Failure<double>("There are no incomes");
    }

    //The created types will be stored in a separate SQL table.
    public async Task<Result<TypeOfIncomes>> AddType(TypeOfIncomesDto typeOfIncomesDto)
    {
        var typeOfIncomesExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfIncomesDto.NameOfType.Trim().ToLower());
        if (typeOfIncomesExist == null)
        {
            TypeOfIncomes typeOfIncomes = new TypeOfIncomes(typeOfIncomesDto.NameOfType);
            await unit.typeOfIncomesRepository.AddAsync(typeOfIncomes);

            await unit.SaveChangesAsync();

            return Result.Success(typeOfIncomes);
        }
        else
            return Result.Failure<TypeOfIncomes>($"Name {typeOfIncomesDto.NameOfType} is already exists, try another name");
    }

    //Updates the name of the specified type,
    // while overwriting the name of all income objects that were marked with the current income type
    public async Task<Result<TypeOfIncomes>> Update(TypeOfIncomesDto typeOfIncomes, string nameOfType)
    {
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
            return Result.Failure<TypeOfIncomes>("Such type of Incomes does not exist");
    }
    //Deletes an income type, which also deletes all income objects associated with that type.

    public async Task<Result<TypeOfIncomes>> Delete(string nameOfType)
    {
        var typeOfIncomes = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeOfIncomes == null)
            return Result.Failure<TypeOfIncomes>("Such type of Incomes does not exist");
        var list = await unit.incomeRepository.GetAll().Where(c => c.TypeOfIncomes == nameOfType).ToListAsync();

        unit.incomeRepository.RemoveRange(list);
        unit.typeOfIncomesRepository.Remove(typeOfIncomes);

        await unit.SaveChangesAsync();

        return Result.Success(typeOfIncomes);
    }
}