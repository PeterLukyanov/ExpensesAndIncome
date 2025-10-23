using Db;
using Repositorys;
using Models;

namespace UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpensesAndIncomesDb _db;
    public IOperationRepository<Expense> expenseRepository { get; }
    public IOperationRepository<Income> incomeRepository { get; }
    public ITypeOfOperationRepository<NameTypeOfExpenses> typeOfExpensesRepository { get; }
    public ITypeOfOperationRepository<NameTypeOfIncomes> typeOfIncomesRepository { get; }
    public IUserRepository userRepository{ get; }

    public UnitOfWork(IOperationRepository<Expense> _expenseRepository,
                         IOperationRepository<Income> _incomeRepository,
                         ITypeOfOperationRepository<NameTypeOfExpenses> _typeOfExpensesRepository,
                         ITypeOfOperationRepository<NameTypeOfIncomes> _typeOfIncomesRepository,
                         ExpensesAndIncomesDb db,
                         IUserRepository _userRepository)
    {
        expenseRepository = _expenseRepository;
        incomeRepository = _incomeRepository;
        typeOfExpensesRepository = _typeOfExpensesRepository;
        typeOfIncomesRepository = _typeOfIncomesRepository;
        _db = db;
        userRepository = _userRepository;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}