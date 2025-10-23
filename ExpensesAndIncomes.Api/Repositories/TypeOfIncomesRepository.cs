using Db;
using Models;

namespace Repositorys;

public class TypeOfIncomesRepository : ITypeOfOperationRepository<NameTypeOfIncomes>
{
    private readonly ExpensesAndIncomesDb _db;

    public TypeOfIncomesRepository(ExpensesAndIncomesDb db)
    {
        _db = db;
    }

    public IQueryable<NameTypeOfIncomes> GetAll()
    {
        return _db.NamesTypesOfIncomes.AsQueryable();
    }

    public async Task AddAsync(NameTypeOfIncomes typeOfIncomes)
    {
        await _db.NamesTypesOfIncomes.AddAsync(typeOfIncomes);
    }

    public void Remove(NameTypeOfIncomes typeOfIncomes)
    {
        _db.NamesTypesOfIncomes.Remove(typeOfIncomes);
    }
}