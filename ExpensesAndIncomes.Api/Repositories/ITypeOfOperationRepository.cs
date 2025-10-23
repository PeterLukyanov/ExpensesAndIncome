using Models;

namespace Repositorys;

public interface ITypeOfOperationRepository<T> where T : NameTypeOfOperations
{
    IQueryable<T> GetAll();
    Task AddAsync(T nameTypeOfOperation);
    void Remove(T nameTypeOfOperation);
}