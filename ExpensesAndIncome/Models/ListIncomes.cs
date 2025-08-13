namespace Models;

public class ListOfIncomes
{
    public double TotalSummOfType { get; set; }
    public List<Income> listOfIncomes { get; set; } = new List<Income>();
    public string NameOfType { get; set; }

    public ListOfIncomes(string nameOfType)
    {
        TotalSummOfType = 0;
        listOfIncomes = new List<Income>();
        NameOfType = nameOfType;
    }
}