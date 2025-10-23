using Models;

namespace Factorys;

public class IncomeFactory : IOperationFactory<Income>
{
    public Income Create(DateTime dateOfAction, double amount, string type, string comment)
    {
        return new Income(dateOfAction, amount, type, comment);
    }
}