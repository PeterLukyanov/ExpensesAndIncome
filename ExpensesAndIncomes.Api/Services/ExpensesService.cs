using Models;
using UoW;
using Repositorys;
using Factorys;

namespace Services;

public class ExpensesService : OperationsService<NameTypeOfExpenses, Expense>
{
    public ExpensesService(IUnitOfWork unit, ILogger<ExpensesService> logger, IOperationFactory<Expense> factory) : base(unit, logger, factory)
    {

    }
    protected override IOperationRepository<Expense> operationRepository => _unit.expenseRepository;
    protected override ITypeOfOperationRepository<NameTypeOfExpenses> typeOfOperationRepository => _unit.typeOfExpensesRepository;
}