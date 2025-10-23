using UoW;
using Factorys;
using Models;
using Repositorys;

namespace Services;

public class ExpensesTypesService : OperationsTypesService<NameTypeOfExpenses, Expense>
{
    public ExpensesTypesService(IUnitOfWork unit, ILogger<OperationsTypesService<NameTypeOfExpenses, Expense>> logger, INameTypeOfOperationsFactory<NameTypeOfExpenses> factory) : base(unit, logger, factory)
    {

    }

    protected override ITypeOfOperationRepository<NameTypeOfExpenses> typeOfOperationRepository => _unit.typeOfExpensesRepository;
    protected override IOperationRepository<Expense> operationRepository => _unit.expenseRepository;
}