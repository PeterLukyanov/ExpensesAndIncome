namespace Models;

public class ListOfExpenses
{
    public double TotalSummOfType { get;  set; }
    public List<Expense> listOfExpenses { get;  set; }
    public string NameOfType { get;  set; }

    public ListOfExpenses(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfExpenses = new List<Expense>();
        NameOfType = nameOfType;
    }

    public void UpdateName(string nameOfType)
    {
        NameOfType = nameOfType;
    }

    public void AddTotalSummOfType(double amount)
    {
        TotalSummOfType += amount;
    }

    public void ReduceTotalSummOfType(double amount)
    {
        TotalSummOfType -= amount;
    }
}
