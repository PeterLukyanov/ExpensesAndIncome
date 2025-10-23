using Db;
using Models;

namespace Repositorys;

public class IncomeRepository : IOperationRepository<Income>
{
    private readonly ExpensesAndIncomesDb _db;

    public IncomeRepository(ExpensesAndIncomesDb db)
    {
        _db = db;
    }

    public IQueryable<Income> GetAll()
    {
        return _db.Incomes.AsQueryable();
    }

    public async Task AddAsync(Income income)
    {
        await _db.Incomes.AddAsync(income);
    }

    public void Remove(Income income)
    {
        _db.Incomes.Remove(income);
    }

    public void RemoveRange(List<Income> incomes)
    {
        _db.Incomes.RemoveRange(incomes);
    }
}