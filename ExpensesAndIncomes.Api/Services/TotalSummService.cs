
using Db;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class TotalSummService
{
    public ExpensesAndIncomesDb db;
    public TotalSummService(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    public async Task<double> TotalBalance()
    {
        double totalIncomes = await db.Incomes.Select(e => e.Amount).SumAsync();
        double totalExpenses = await db.Expenses.Select(e => e.Amount).SumAsync();
        return totalIncomes - totalExpenses;
    }

}