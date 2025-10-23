using Models;

namespace Factorys;

public class ExpenseFactory : IOperationFactory<Expense>
{
    public Expense Create(DateTime dateOfAction, double amount, string type, string comment)
    {
        return new Expense(dateOfAction, amount, type, comment);
    }
}