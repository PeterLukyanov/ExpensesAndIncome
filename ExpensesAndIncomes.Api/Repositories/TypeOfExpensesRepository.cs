using Db;
using Models;

namespace Repositorys;

public class TypeOfExpensesRepository : ITypeOfExpensesRepository
{
    private readonly ExpensesAndIncomesDb db;
    public TypeOfExpensesRepository(ExpensesAndIncomesDb _db)
    {
        db = _db;
    }
    public IQueryable<TypeOfExpenses> GetAll()
    {
        return db.TypesOfExpenses.AsQueryable();
    }

    public async Task AddAsync(TypeOfExpenses typeOfExpenses)
    {
        await db.TypesOfExpenses.AddAsync(typeOfExpenses);
    }

    public void Remove(TypeOfExpenses typeOfExpenses)
    {
        db.TypesOfExpenses.Remove(typeOfExpenses);
    }
}