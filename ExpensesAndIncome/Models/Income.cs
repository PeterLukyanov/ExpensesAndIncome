namespace Models;

public class Income 
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string StringTypeOfIncomes { get; private set; }
    public string Comment { get; private set; }
    public Income(DateTime dataOfAction, double amount, string stringTypeOfIncomes, string comment)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        StringTypeOfIncomes = stringTypeOfIncomes;
        Comment = comment;
    }
}