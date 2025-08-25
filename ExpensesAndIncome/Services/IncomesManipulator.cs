using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services;

//This service is for getting information on ID, deleting, or creating new income.

public class IncomesManipulator
{
    public ExpensesAndIncomesDb db;

    public IncomesManipulator(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    //Displays a list of all incomes
    public async Task<List<Income>> InfoOfIncomes()
    {
        var list = await db.Incomes.ToListAsync();

        if (list.Count == 0)
            return null!;
        else
        {
            return list;
        }
    }
//Adds income to existing income categories
    public async Task AddNewIncome(IncomeDto dto)
    {
        Income newIncome = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);

        await db.Incomes.AddAsync(newIncome);
        await db.SaveChangesAsync();
    }

//Deletes a specific income by ID
    public async Task Delete(int id)
    {
        var Income = await db.Incomes.FindAsync(id);
        db.Incomes.Remove(Income!);
        await db.SaveChangesAsync();
    }
}