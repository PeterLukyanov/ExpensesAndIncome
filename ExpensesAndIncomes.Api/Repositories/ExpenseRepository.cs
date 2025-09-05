using Db;
using Models;

namespace Repositorys;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpensesAndIncomesDb db;

    public ExpenseRepository(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    public IQueryable<Expense> GetAll()
    {
        return db.Expenses.AsQueryable();
    }

    public async Task AddAsync(Expense expense)
    {
        await db.Expenses.AddAsync(expense);
    }

    public void Remove(Expense expense)
    {
        db.Expenses.Remove(expense);
    }

    public void RemoveRange(List<Expense> expenses)
    {
        db.Expenses.RemoveRange(expenses);
    }
}