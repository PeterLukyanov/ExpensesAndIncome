namespace Models;

public class ListOfIncomes
{
    public double TotalSummOfType { get; private set; }
    public List<Income> listOfIncomes { get; private set; }
    public string NameOfType { get; private set; }

    public ListOfIncomes(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfIncomes = new List<Income>();
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