namespace Models;

public class Income : Operation
{
    public Income(DateTime dataOfAction, double amount, string type, string comment) :base(dataOfAction, amount, type, comment)
    {
    }
}