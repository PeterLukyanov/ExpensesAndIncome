using Models;

namespace Factorys;

public class NameTypeOfExpensesFactory : INameTypeOfOperationsFactory<NameTypeOfExpenses>
{
    public NameTypeOfExpenses Create(string nameOfType)
    {
        return new NameTypeOfExpenses(nameOfType);
    }
}