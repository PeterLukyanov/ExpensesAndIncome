using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using CSharpFunctionalExtensions;

namespace Services;

//This service is for getting information on ID, deleting, or creating new income.

public class IncomesManipulator
{
    private readonly ExpensesAndIncomesDb db;

    public IncomesManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }

    //Displays a list of all incomes
    public async Task<Result<List<Income>>> InfoOfIncomes()
    {
        var list = await db.Incomes.ToListAsync();

        if (list.Count == 0)
            return Result.Failure<List<Income>>("There are no Incomes for now");
        else
        {
            return Result.Success(list);
        }
    }

    //Adds income to existing income categories
    public async Task<Result<Income>> AddNewIncome(IncomeDto dto)
    {
        var typeList = await db.TypesOfIncomes.FirstOrDefaultAsync(t => t.Name == dto.TypeOfIncomes);
        if (typeList == null)
        {
            return Result.Failure<Income>("This type of Incomes does not found");
        }

        Income newIncome = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);

        await db.Incomes.AddAsync(newIncome);
        await db.SaveChangesAsync();
        return Result.Success(newIncome);
    }

    //Update a specific income by Id
    public async Task<Result<Income>> Update(IncomeDto dto, int Id)
    {
        var income = await db.Incomes.FirstOrDefaultAsync(e => e.Id == Id);
        if (income == null)
        {
            return Result.Failure<Income>($"Income whith this Id({Id}) does not exist");
        }
        income.UpdateAmount(dto.Amount);
        income.UpdateComment(dto.Comment);
        income.UpdateTypeOfIncomes(dto.TypeOfIncomes);

        await db.SaveChangesAsync();

        return Result.Success(income);
    }

    //Deletes a specific income by ID
    public async Task<Result<Income>> Delete(int id)
    {
        var item = await db.Incomes.FirstOrDefaultAsync(c => c.Id == id);
        if (item == null)
        {
            return Result.Failure<Income>($"Income whith this Id({id}) does not exist");
        }
        db.Incomes.Remove(item);
        await db.SaveChangesAsync();
        return Result.Success(item);
    }
}