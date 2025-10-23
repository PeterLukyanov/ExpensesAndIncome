using Db;
using Models;

namespace Repositorys;

public class TypeOfExpensesRepository : ITypeOfOperationRepository<NameTypeOfExpenses>
{
    private readonly ExpensesAndIncomesDb _db;
    public TypeOfExpensesRepository(ExpensesAndIncomesDb db)
    {
        _db = db;
    }
    public IQueryable<NameTypeOfExpenses> GetAll()
    {
        return _db.NamesTypesOfExpenses.AsQueryable();
    }

    public async Task AddAsync(NameTypeOfExpenses nameTypeOfExpenses)
    {
        await _db.NamesTypesOfExpenses.AddAsync(nameTypeOfExpenses);
    }

    public void Remove(NameTypeOfExpenses nameTypeOfExpenses)
    {
        _db.NamesTypesOfExpenses.Remove(nameTypeOfExpenses);
    }
}