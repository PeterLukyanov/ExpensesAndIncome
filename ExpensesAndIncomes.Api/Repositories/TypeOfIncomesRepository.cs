using Db;
using Models;

namespace Repositorys;

public class TypeOfIncomesRepository : ITypeOfIncomesRepository
{
    private readonly ExpensesAndIncomesDb db;

    public TypeOfIncomesRepository(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }

    public IQueryable<TypeOfIncomes> GetAll()
    {
        return db.TypesOfIncomes.AsQueryable();
    }

    public async Task AddAsync(TypeOfIncomes typeOfIncomes)
    {
        await db.TypesOfIncomes.AddAsync(typeOfIncomes);
    }

    public void Remove(TypeOfIncomes typeOfIncomes)
    {
        db.TypesOfIncomes.Remove(typeOfIncomes);
    }
}