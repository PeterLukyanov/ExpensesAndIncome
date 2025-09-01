using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;

//This service is for working with a type: it allows you to load start types,
// add a new one, view all types, view all information about a specific type,
// view the total amount of expenses, change a type, delete a type

namespace Services;

public class ExpensesTypesManipulator
{
    private readonly ExpensesAndIncomesDb db;
    public ExpensesTypesManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
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
    public async Task<Result<List<TypeOfExpenses>>> InfoTypes()
    {
        var typesOfExpenses = await db.TypesOfExpenses.ToListAsync();
        if (typesOfExpenses.Count == 0)
        {
            return Result.Failure<List<TypeOfExpenses>>("There are no types of Expenses for now");
        }
        return Result.Success(typesOfExpenses);
    }

    //Displays information about the type by name, namely: the sum of all expenses by type,
    // and a list of all expense objects
    public async Task<Result<ListOfExpenses>> GetInfoOfType(string type)
    {
        var typesOfExpenses = await db.TypesOfExpenses.FirstOrDefaultAsync(t => t.Name == type);
        if (typesOfExpenses != null)
        {
            ListOfExpenses listOfExpenses = new ListOfExpenses(type);
            listOfExpenses.AddTotalSummOfType(await db.Expenses
                .Where(e => e.TypeOfExpenses == type)
                .SumAsync(e => e.Amount));
            listOfExpenses.UpdateListOfExpenses(await db.Expenses
                .Where(e => e.TypeOfExpenses == type)
                .ToListAsync());
            return Result.Success(listOfExpenses);
        }
        else
            return Result.Failure<ListOfExpenses>("Such type of Expenses does not exist");
    }

    //Displays the total amount of all expenses
    public async Task<Result<double>> TotalSummOfExpenses()
    {
        bool expensesExist = await db.Expenses.AnyAsync();
        if (expensesExist)
        {
            return Result.Success(await db.Expenses.Select(e => e.Amount)
                                                    .SumAsync());
        }
        else
            return Result.Failure<double>("There are no expenses");
    }

    //Created types will be stored in a separate SQL table.
    public async Task<Result<TypeOfExpenses>> AddType(TypeOfExpensesDto typeOfExpensesDto)
    {
        var typeOfExpensesExist = await db.TypesOfExpenses.FirstOrDefaultAsync(t => t.Name.ToLower() == typeOfExpensesDto.NameOfType.Trim().ToLower());
        if (typeOfExpensesExist == null)
        {
            TypeOfExpenses typeOfExpenses = new TypeOfExpenses(typeOfExpensesDto.NameOfType);
            await db.TypesOfExpenses.AddAsync(typeOfExpenses);

            await db.SaveChangesAsync();

            return Result.Success(typeOfExpenses);
        }
        else
            return Result.Failure<TypeOfExpenses>($"Name {typeOfExpensesDto.NameOfType} is already exists, try another name");
    }

    //Updates the name of the specified type,
    // while overwriting the name of all expense objects that were marked with the current expense type
    public async Task<Result<TypeOfExpenses>> Update(TypeOfExpensesDto typeOfExpenses, string nameOfType)
    {
        var typeExist = await db.TypesOfExpenses.FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeExist != null)
        {
            var listOfExpenses = await db.Expenses.Where(e => e.TypeOfExpenses == nameOfType).ToListAsync();

            for (int i = 0; i < listOfExpenses.Count; i++)
            {
                listOfExpenses[i].UpdateTypeOfExpenses(typeOfExpenses.NameOfType);
            }

            typeExist.UpdateName(nameOfType);
            await db.SaveChangesAsync();
            return Result.Success(typeExist);
        }
        else
            return Result.Failure<TypeOfExpenses>("Such type of Expenses does not exist");
    }

    //Deletes an expense type, which also deletes all expense objects associated with that type.
    public async Task<Result<TypeOfExpenses>> Delete(string nameOfType)
    {
        var typeOfExpenses = await db.TypesOfExpenses.FirstOrDefaultAsync(t => t.Name == nameOfType);
        if (typeOfExpenses == null)
            return Result.Failure<TypeOfExpenses>("Such type of Expenses does not exist");
        var list = await db.Expenses.Where(c => c.TypeOfExpenses == nameOfType).ToListAsync();

        db.Expenses.RemoveRange(list);
        db.TypesOfExpenses.Remove(typeOfExpenses);

        await db.SaveChangesAsync();

        return Result.Success(typeOfExpenses);
    }
}
