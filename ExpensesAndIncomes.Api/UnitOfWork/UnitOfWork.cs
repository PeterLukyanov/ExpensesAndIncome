using Db;
using Repositorys;

namespace UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpensesAndIncomesDb db;
    public IExpenseRepository expenseRepository { get; }
    public IIncomeRepository incomeRepository { get; }
    public ITypeOfExpensesRepository typeOfExpensesRepository { get; }
    public ITypeOfIncomesRepository typeOfIncomesRepository { get; }
    public IUserRepository userRepository{ get; }

    public UnitOfWork(IExpenseRepository _expenseRepository, IIncomeRepository _incomeRepository, ITypeOfExpensesRepository _typeOfExpensesRepository, ITypeOfIncomesRepository _typeOfIncomesRepository, ExpensesAndIncomesDb _db, IUserRepository _userRepository)
    {
        expenseRepository = _expenseRepository;
        incomeRepository = _incomeRepository;
        typeOfExpensesRepository = _typeOfExpensesRepository;
        typeOfIncomesRepository = _typeOfIncomesRepository;
        db = _db;
        userRepository = _userRepository;
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}