namespace Models;

public class ListTypesOfIncomes
{
    public double TotalSummOfIncomes { get; private set; }
    public List<ListOfIncomes> listTypeOfIncomes { get; private set; }


    public ListTypesOfIncomes()
    {
        TotalSummOfIncomes = 0;
        listTypeOfIncomes = new List<ListOfIncomes>();
    }

    public void UpdateTypesOfIncomes(List<ListOfIncomes> _listTypeOfIncomes)
    {
        listTypeOfIncomes = _listTypeOfIncomes;
    }

    public void AddTotalSumm(double amount)
    {
        TotalSummOfIncomes += amount;
    }

    public void ReduceTotalSumm(double amount)
    {
        TotalSummOfIncomes -= amount;
    }
}