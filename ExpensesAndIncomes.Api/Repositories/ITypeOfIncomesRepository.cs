using Models;

namespace Repositorys;

public interface ITypeOfIncomesRepository
{
    IQueryable<TypeOfIncomes> GetAll();
    Task AddAsync(TypeOfIncomes typeOfIncomes);
    void Remove(TypeOfIncomes typeOfIncomes);
}