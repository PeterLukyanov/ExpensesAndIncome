using Dtos;
using Models;
using Db;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;

namespace Services;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of incomes, change a type, delete a type

public class IncomesTypeManipulator
{
    private readonly ExpensesAndIncomesDb db;
    public IncomesTypeManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }

    //Checks if there are any incomes in the program, if not, it will load the starting types
    public async Task LoadTypeOfIncomes()
    {
        bool incomesExist = await db.TypesOfIncomes.AnyAsync();
        if (!incomesExist)
        {
            await db.TypesOfIncomes.AddAsync(new TypeOfIncomes("Salary"));
            await db.TypesOfIncomes.AddAsync(new TypeOfIncomes("Other"));
            await db.SaveChangesAsync();
        }
    }
    //Displays all types of incomes that are in the program
    public async Task<Result<List<TypeOfIncomes>>> InfoTypes()
    {
        var typesOfIncomes = await db.TypesOfIncomes.ToListAsync();
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
        var typesOfIncomes = await db.TypesOfIncomes.FirstOrDefaultAsync(t => t.Name == type);

        if (typesOfIncomes != null)
        {
            ListOfIncomes listOfIncomes = new ListOfIncomes(type);
            listOfIncomes.AddTotalSummOfType(await db.Incomes
                .Where(i => i.TypeOfIncomes == type)
                .SumAsync(e => e.Amount));
            listOfIncomes.UpdateListOfIncomes(await db.Incomes
                .Where(i => i.TypeOfIncomes == type)
                .ToListAsync());
            return Result.Success(listOfIncomes);
        }
        return Result.Failure<ListOfIncomes>("Such type of Incomes does not exist");
    }

    //Displays the total amount of all incomes
    public async Task<Result<double>> TotalSummOfIncomes()
    {
        bool incomesExist = await db.Incomes.AnyAsync();
        if (incomesExist)
        {
            return Result.Success(await db.Incomes.Select(e => e.Amount)
                                        .SumAsync());
        }
        else
            return Result.Failure<double>("There are no incomes");
    }

    //The created types will be stored in a separate SQL table.
    public async Task<Result<TypeOfIncomes>> AddType(TypeOfIncomesDto typeOfIncomesDto)
    {
        var typeOfIncomesExist = await db.TypesOfIncomes.FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfIncomesDto.NameOfType.Trim().ToLower());
        if (typeOfIncomesExist == null)
        {
            TypeOfIncomes typeOfIncomes = new TypeOfIncomes(typeOfIncomesDto.NameOfType);
            await db.TypesOfIncomes.AddAsync(typeOfIncomes);

            await db.SaveChangesAsync();

            return Result.Success(typeOfIncomes);
        }
        else
            return Result.Failure<TypeOfIncomes>($"Name {typeOfIncomesDto.NameOfType} is already exists, try another name");
    }

    //Updates the name of the specified type,
    // while overwriting the name of all income objects that were marked with the current income type
    public async Task<Result<TypeOfIncomes>> Update(TypeOfIncomesDto typeOfIncomes, string nameOfType)
    {
        var typeExist = await db.TypesOfIncomes.FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeExist != null)
        {
            var listOfIncomes = await db.Incomes.Where(e => e.TypeOfIncomes == nameOfType).ToListAsync();

            for (int i = 0; i < listOfIncomes.Count; i++)
            {
                listOfIncomes[i].UpdateTypeOfIncomes(typeOfIncomes.NameOfType);
            }

            typeExist.UpdateName(nameOfType);
            await db.SaveChangesAsync();
            return Result.Success(typeExist);
        }
        else
            return Result.Failure<TypeOfIncomes>("Such type of Incomes does not exist");
    }
    //Deletes an income type, which also deletes all income objects associated with that type.

    public async Task<Result<TypeOfIncomes>> Delete(string nameOfType)
    {
        var typeOfIncomes = await db.TypesOfIncomes.FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeOfIncomes == null)
            return Result.Failure<TypeOfIncomes>("Such type of Incomes does not exist");
        var list = await db.Incomes.Where(c => c.TypeOfIncomes == nameOfType).ToListAsync();

        db.Incomes.RemoveRange(list);
        db.TypesOfIncomes.Remove(typeOfIncomes);

        await db.SaveChangesAsync();

        return Result.Success(typeOfIncomes);
    }
}