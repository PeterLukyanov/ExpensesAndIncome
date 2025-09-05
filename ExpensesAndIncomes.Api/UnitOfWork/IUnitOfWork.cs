using Repositorys;

namespace UoW;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    IExpenseRepository expenseRepository { get; }
    IIncomeRepository incomeRepository { get; }
    ITypeOfExpensesRepository typeOfExpensesRepository { get; }
    ITypeOfIncomesRepository typeOfIncomesRepository{ get; }
}