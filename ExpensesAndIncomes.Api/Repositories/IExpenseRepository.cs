using Models;

namespace Repositorys;

public interface IExpenseRepository
{
    IQueryable<Expense> GetAll();
    Task AddAsync(Expense expense);
    void Remove(Expense expense);
    void RemoveRange(List<Expense> expenses);
}