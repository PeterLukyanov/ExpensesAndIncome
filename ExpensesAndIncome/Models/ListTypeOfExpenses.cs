namespace Models;

public class ListTypesOfExpenses
{
    public double TotalSummOfExpenses { get; set; }

    public List<ListOfExpenses> listTypeOfExpenses { get; set; } = new List<ListOfExpenses>();

    public ListTypesOfExpenses()
    {
        TotalSummOfExpenses = 0;
        listTypeOfExpenses = new List<ListOfExpenses>();
    }
}