namespace Models;

public abstract class Operation
{
    public DateTime DataOfAction { get; private set; }
    public double Amount { get; private set; }
    public string Type { get; private set; } = null!;
    public string Comment { get; private set; } = null!;
    public int Id { get; private set; }
    public Operation(DateTime dataOfAction, double amount, string type, string comment)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        Type = type;
        Comment = comment;
    }

    public void UpdateType(string newType)
    {
        Type = newType;
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