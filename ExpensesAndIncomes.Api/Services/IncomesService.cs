using Models;
using UoW;
using Factorys;
using Repositorys;

namespace Services;

public class IncomesService : OperationsService<NameTypeOfIncomes, Income>
{
    public IncomesService(IUnitOfWork unit, ILogger<IncomesService> logger, IOperationFactory<Income> factory) : base(unit, logger, factory)
    {
    }

    protected override IOperationRepository<Income> operationRepository => _unit.incomeRepository;
    protected override ITypeOfOperationRepository<NameTypeOfIncomes> typeOfOperationRepository => _unit.typeOfIncomesRepository;
    
}