using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of expenses, change a type, delete a type

namespace Services;

public class ExpensesTypesManipulator
{
    public ExpensesAndIncomesDb db;
    public ExpensesTypesManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }

    //The created types will be stored in a separate SQL table.

    public async Task AddType(TypeOfExpensesDto listOfExpensesDto)
    {
        var typesOfExpenses = await db.TypesOfExpenses.Select(e => e.Name).ToListAsync();
        typesOfExpenses.Add(listOfExpensesDto.NameOfType);

        await db.SaveChangesAsync();
    }

    //Checks if there are any expenses in the program, if not, it will load the starting types
    public async Task LoadTypeOfExpenses()
    {
        bool expensesList = await db.TypesOfExpenses.AnyAsync();
        if (!expensesList)
        {
            await db.TypesOfExpenses.AddAsync(new TypeOfExpenses("Food"));
            await db.TypesOfExpenses.AddAsync(new TypeOfExpenses("Relax"));
            await db.SaveChangesAsync();
        }
    }

    //Displays all types of expenses that are in the program

    public async Task<List<string>> InfoTypes()
    {
        return await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
    }

    //Displays information about the type by name, namely: the sum of all expenses by type,
    // and a list of all expense objects

    public async Task<ListOfExpenses> GetInfoOfType(string type)
    {
        var typesOfExpenses = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        foreach (var typeOfExpenses in typesOfExpenses)
        {
            if (typeOfExpenses == type)
            {
                ListOfExpenses listOfExpenses = new ListOfExpenses(type);
                listOfExpenses.AddTotalSummOfType(await db.Expenses
                    .Where(e => e.TypeOfExpenses == type)
                    .SumAsync(e => e.Amount));
                listOfExpenses.UpdateListOfExpenses(await db.Expenses
                    .Where(e => e.TypeOfExpenses == type)
                    .ToListAsync());
                return listOfExpenses;
            }
        }

        return null!;
    }

    //Displays the total amount of all expenses

    public async Task<double> TotalSummOfExpenses()
    {
        return await db.Expenses.Select(e => e.Amount)
                                .SumAsync();
    }

    //Updates the name of the specified type,
    // while overwriting the name of all expense objects that were marked with the current expense type

    public async Task Update(TypeOfExpensesDto listOfExpenses, string nameOfType)
    {
        await db.Expenses.Where(e => e.TypeOfExpenses == listOfExpenses.NameOfType)
                         .ExecuteUpdateAsync(e => e.SetProperty(x => x.TypeOfExpenses, nameOfType));
        var item = await db.TypesOfExpenses.Select(t => t.Name).ToListAsync();
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i] == listOfExpenses.NameOfType)
            {
                item[i] = nameOfType;
            }
        }
        await db.SaveChangesAsync();
    }

    //Deletes an expense type, which also deletes all expense objects associated with that type.
    
    public async Task Delete(string nameOfType)
    {
        var listOfExpenses = GetInfoOfType(nameOfType);
        if (listOfExpenses == null)
            return;

        var list = await db.Expenses.Where(c => c.TypeOfExpenses == nameOfType).ToListAsync();

        db.Expenses.RemoveRange(list);
        await db.SaveChangesAsync();

        var item = await db.TypesOfExpenses.Where(t => t.Name == nameOfType).ToListAsync();

        db.TypesOfExpenses.RemoveRange(item!);
    }
}
