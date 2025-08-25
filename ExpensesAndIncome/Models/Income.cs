namespace Models;

public class Income
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string TypeOfIncomes { get; private set; }
    public string Comment { get; private set; }
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
}