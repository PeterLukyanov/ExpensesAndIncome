namespace Models;

// This model is needed for structured output of information from SQL
public class ListOfExpenses
{
    public double TotalSummOfType { get; private set; }
    public List<Expense> listOfExpenses { get; private set; } = null!;
    public string NameOfType { get; private set; } = null!;
    public ListOfExpenses(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfExpenses = new List<Expense>();
        NameOfType = nameOfType;
    }
    public void UpdateListOfExpenses(List<Expense> expenses)
    {
        listOfExpenses = expenses;
    }

    public void UpdateName(string nameOfType)
    {
        NameOfType = nameOfType;
    }

    public void AddTotalSummOfType(double amount)
    {
        TotalSummOfType += amount;
    }

}
