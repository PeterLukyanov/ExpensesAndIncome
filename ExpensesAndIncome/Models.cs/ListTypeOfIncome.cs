namespace Models;

public class ListTypesOfIncomes
{
    public double TotalSummOfIncomes { get; set; }
    public List<ListOfIncomes> listTypeOfIncomes { get; set; } = new List<ListOfIncomes>();


    public ListTypesOfIncomes()
    {
        TotalSummOfIncomes = 0;
        listTypeOfIncomes = new List<ListOfIncomes>();
    }
}