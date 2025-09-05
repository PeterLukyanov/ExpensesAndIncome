using Models;

namespace Repositorys;

public interface IIncomeRepository
{
    IQueryable<Income> GetAll();

    Task AddAsync(Income income);

    void Remove(Income income);

    void RemoveRange(List<Income> incomes);
}