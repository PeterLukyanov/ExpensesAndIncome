namespace Models;

public class Expense : Operation
{
    public Expense(DateTime dataOfAction, double amount, string type, string comment) : base(dataOfAction, amount,type,comment)
    {
    }
}