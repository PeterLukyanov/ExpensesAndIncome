using Db;
using Models;

namespace Repositorys;

public class IncomeRepository : IIncomeRepository
{
    private readonly ExpensesAndIncomesDb db;

    public IncomeRepository(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }

    public IQueryable<Income> GetAll()
    {
        return db.Incomes.AsQueryable();
    }

    public async Task AddAsync(Income income)
    {
        await db.Incomes.AddAsync(income);
    }

    public void Remove(Income income)
    {
        db.Incomes.Remove(income);
    }

    public void RemoveRange(List<Income> incomes)
    {
        db.Incomes.RemoveRange(incomes);
    }
}