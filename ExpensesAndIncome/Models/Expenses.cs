namespace Models;

public class Expense
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string TypeOfExpenses { get; private set; }
    public string Comment { get; private set; }
    public int Id { get; private set; }

    public Expense(DateTime dataOfAction, double amount, string typeOfExpenses, string comment, int id)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        TypeOfExpenses = typeOfExpenses;
        Comment = comment;
        Id = id;
    }

    public void UpdateTypeOfExpenses(string typeOfExpenses)
    {
        TypeOfExpenses = typeOfExpenses;
    }
}