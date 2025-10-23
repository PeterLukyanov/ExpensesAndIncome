using Models;

namespace Repositorys;

public interface IOperationRepository<T> where T:Operation
{
    IQueryable<T> GetAll();
    Task AddAsync(T operation);
    void Remove(T operation);
    void RemoveRange(List<T> operations);
}