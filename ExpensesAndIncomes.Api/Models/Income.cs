namespace Models;

public class Income
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string TypeOfIncomes { get; private set; } = null!;
    public string Comment { get; private set; } = null!;
    public int Id { get; private set; }
    public Income(DateTime dataOfAction, double amount, string typeOfIncomes, string comment)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        TypeOfIncomes = typeOfIncomes;
        Comment = comment;
    }

    public void UpdateTypeOfIncomes(string typeOfIncomes)
    {
        TypeOfIncomes = typeOfIncomes;
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