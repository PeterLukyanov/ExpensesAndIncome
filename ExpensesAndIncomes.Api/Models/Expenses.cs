namespace Models;

public class Expense
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string TypeOfExpenses { get; private set; } = null!;
    public string Comment { get; private set; } = null!;
    public int Id { get; private set; }

    public Expense(DateTime dataOfAction, double amount, string typeOfExpenses, string comment)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        TypeOfExpenses = typeOfExpenses;
        Comment = comment;
    }

    public void UpdateTypeOfExpenses(string typeOfExpenses)
    {
        TypeOfExpenses = typeOfExpenses;
    }

    public void UpdateAmount(double amount)
    {
        Amount = amount;
    }

    public void UpdateComment(string comment)
    {
        Comment = comment;
    }

    

}