namespace Models;

public class Income : IExpensesOrIncome
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string StringTypeOfExpenses { get; private set; }
    public string Comment { get; private set; }
    public Income(DateTime dataOfAction, double amount, string stringTypeOfExpenses, string comment)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        StringTypeOfExpenses = stringTypeOfExpenses;
        Comment = comment;
    }
}