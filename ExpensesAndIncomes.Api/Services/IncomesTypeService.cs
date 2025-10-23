using Models;
using UoW;
using Factorys;
using Repositorys;

namespace Services;

public class IncomesTypesService : OperationsTypesService<NameTypeOfIncomes, Income>
{
    public IncomesTypesService(IUnitOfWork unit, ILogger<IncomesTypesService> logger, INameTypeOfOperationsFactory<NameTypeOfIncomes> factory) : base(unit, logger, factory)
    {
        
    }

    protected override ITypeOfOperationRepository<NameTypeOfIncomes> typeOfOperationRepository => _unit.typeOfIncomesRepository;
    protected override IOperationRepository<Income> operationRepository => _unit.incomeRepository;
}