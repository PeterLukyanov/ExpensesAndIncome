using Dtos;
using Models;
using Db;
using Microsoft.EntityFrameworkCore;

namespace Services;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of incomes, change a type, delete a type

public class IncomesTypeManipulator
{
    public ExpensesAndIncomesDb db;
    public IncomesTypeManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    //The created types will be stored in a separate SQL table.
    public async Task AddType(TypeOfIncomesDto listOfIncomesDto)
    {
        var typesOfIncomes = await db.TypesOfIncomes.Select(e => e.Name).ToListAsync();
        typesOfIncomes.Add(listOfIncomesDto.NameOfType);

        await db.SaveChangesAsync();
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
    public async Task<List<string>> InfoTypes()
    {
        return await db.TypesOfIncomes.Select(t => t.Name).ToListAsync();
    }

    //Displays information about the type by name, namely: the sum of all incomes by type,
    // and a list of all income objects
    public async Task<ListOfIncomes> GetInfoOfType(string type)
    {
        var typesOfIncomes = await db.TypesOfIncomes.Select(t => t.Name).ToListAsync();
        foreach (var typeOfIncomes in typesOfIncomes)
        {
            if (typeOfIncomes == type)
            {
                ListOfIncomes listOfIncomes = new ListOfIncomes(type);
                listOfIncomes.AddTotalSummOfType(await db.Incomes
                    .Where(i => i.TypeOfIncomes == type)
                    .SumAsync(e => e.Amount));
                listOfIncomes.UpdateListOfIncomes(await db.Incomes
                    .Where(i => i.TypeOfIncomes == type)
                    .ToListAsync());
                return listOfIncomes;
            }
        }
        return null!;
    }
    //Displays the total amount of all incomes
    public async Task<double> TotalSummOfIncomes()
    {
        return await db.Incomes.Select(e => e.Amount)
                                .SumAsync();
    }
//Updates the name of the specified type,
    // while overwriting the name of all income objects that were marked with the current income type
    public async Task Update(TypeOfIncomesDto listOfIncomes, string nameOfType)
    {
        await db.Incomes.Where(e => e.TypeOfIncomes == listOfIncomes.NameOfType)
                         .ExecuteUpdateAsync(e => e.SetProperty(x => x.TypeOfIncomes, nameOfType));
        var item = await db.TypesOfIncomes.Select(t => t.Name).ToListAsync();
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i] == listOfIncomes.NameOfType)
            {
                item[i] = nameOfType;
            }
        }
        await db.SaveChangesAsync();
    }
//Deletes an income type, which also deletes all income objects associated with that type.
    
    public async Task Delete(string nameOfType)
    {
        var listOfIncomes = GetInfoOfType(nameOfType);
        if (listOfIncomes == null)
            return;

        var list = await db.Incomes.Where(c => c.TypeOfIncomes == nameOfType).ToListAsync();

        db.Incomes.RemoveRange(list);
        await db.SaveChangesAsync();

        var item = await db.TypesOfIncomes.Where(t => t.Name == nameOfType).ToListAsync();

        db.TypesOfIncomes.RemoveRange(item!);
    }
}