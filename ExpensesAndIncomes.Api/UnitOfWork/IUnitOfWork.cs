using Models;
using Repositorys;

namespace UoW;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    IOperationRepository<Expense> expenseRepository { get; }
    IOperationRepository<Income> incomeRepository { get; }
    ITypeOfOperationRepository<NameTypeOfExpenses> typeOfExpensesRepository { get; }
    ITypeOfOperationRepository<NameTypeOfIncomes> typeOfIncomesRepository { get; }
    IUserRepository userRepository{ get; }
}