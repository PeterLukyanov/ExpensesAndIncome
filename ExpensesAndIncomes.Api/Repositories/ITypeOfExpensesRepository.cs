using Models;

namespace Repositorys;

public interface ITypeOfExpensesRepository
{
    IQueryable<TypeOfExpenses> GetAll();
    Task AddAsync(TypeOfExpenses typeOfExpenses);
    void Remove(TypeOfExpenses typeOfExpenses);
}