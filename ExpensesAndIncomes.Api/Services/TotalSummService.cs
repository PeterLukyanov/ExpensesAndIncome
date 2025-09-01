
using Db;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;

namespace Services;

public class TotalSummService
{
    private readonly ExpensesAndIncomesDb db;
    public TotalSummService(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    public async Task<Result<double>> TotalBalance()
    {
        bool existExpenses = await db.Expenses.AnyAsync();
        bool existIncomes = await db.Incomes.AnyAsync();
        if (existExpenses && existIncomes)
        {
            double totalIncomes = await db.Incomes.Select(e => e.Amount).SumAsync();
            double totalExpenses = await db.Expenses.Select(e => e.Amount).SumAsync();
            return Result.Success(totalIncomes - totalExpenses);
        }
        else
            return Result.Failure<double>("There are no expense and incomes");
    }

}