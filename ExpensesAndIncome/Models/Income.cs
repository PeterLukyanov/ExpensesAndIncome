namespace Models;

public class Income 
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string StringTypeOfIncomes { get;  set; }
    public string Comment { get; private set; }
    public int Id { get; set; }
    public Income(DateTime dataOfAction, double amount, string stringTypeOfIncomes, string comment, int id)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        StringTypeOfIncomes = stringTypeOfIncomes;
        Comment = comment;
        Id = id;
    }
}