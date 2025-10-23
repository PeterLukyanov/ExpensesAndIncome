using Models;

namespace Factorys;

public class NameTypeOfIncomesFactory : INameTypeOfOperationsFactory<NameTypeOfIncomes>
{
    public NameTypeOfIncomes Create(string nameOfType)
    {
        return new NameTypeOfIncomes(nameOfType);
    }
}