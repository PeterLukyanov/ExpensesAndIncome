namespace Models;

// This model is needed for structured output of information from SQL
public class ListOfIncomes
{
    public double TotalSummOfType { get; private set; }
    public List<Income> listOfIncomes { get; private set; } = null!;
    public string NameOfType { get; private set; } = null!;

    public ListOfIncomes(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfIncomes = new List<Income>();
        NameOfType = nameOfType;
    }
    public void UpdateListOfIncomes(List<Income> incomes)
    {
        listOfIncomes = incomes;
    }
    public void UpdateName(string nameOfType)
    {
        NameOfType = nameOfType;
    }

    public void AddTotalSummOfType(double amount)
    {
        TotalSummOfType = amount;
    }
}