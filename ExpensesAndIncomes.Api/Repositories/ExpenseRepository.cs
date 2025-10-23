using Db;
using Models;

namespace Repositorys;

public class ExpenseRepository : IOperationRepository<Expense>
{
    private readonly ExpensesAndIncomesDb _db;

    public ExpenseRepository(ExpensesAndIncomesDb db)
    {
        _db = db;
    }
    public IQueryable<Expense> GetAll()
    {
        return _db.Expenses.AsQueryable();
    }

    public async Task AddAsync(Expense expense)
    {
        await _db.Expenses.AddAsync(expense);
    }

    public void Remove(Expense expense)
    {
        _db.Expenses.Remove(expense);
    }

    public void RemoveRange(List<Expense> expenses)
    {
        _db.Expenses.RemoveRange(expenses);
    }
}