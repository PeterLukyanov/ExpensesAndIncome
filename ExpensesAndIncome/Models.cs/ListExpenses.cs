namespace Models;

public class ListOfExpenses
{
    public double TotalSummOfType { get;  set; }
    public List<Expense> listOfExpenses{ get; set; } = new List<Expense>();
    public string NameOfType{ get; set; }

    public ListOfExpenses(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfExpenses = new List<Expense>();
        NameOfType = nameOfType;
    }
}