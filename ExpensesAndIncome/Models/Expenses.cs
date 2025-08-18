using System.Text.Json.Serialization;

namespace Models;

public class Expense
{
    public DateTime DataOfAction { get; set; }
    public double Amount { get; private set; }
    public string StringTypeOfExpenses { get; set; }
    public string Comment { get; private set; }
    public int Id { get;  set; }

    public Expense(DateTime dataOfAction, double amount, string stringTypeOfExpenses, string comment, int id)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        StringTypeOfExpenses = stringTypeOfExpenses;
        Comment = comment;
        Id = id;
    }
}