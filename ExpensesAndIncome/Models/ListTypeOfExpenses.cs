namespace Models;

public class ListTypesOfExpenses
{
    public double TotalSummOfExpenses { get; private set; }

    public List<ListOfExpenses> listTypeOfExpenses { get; private set; }

    public ListTypesOfExpenses()
    {
        TotalSummOfExpenses = 0;
        listTypeOfExpenses = new List<ListOfExpenses>();
    }

    public void UpdateTypesOfExpenses(List<ListOfExpenses> _listTypeOfExpeses)
    {
        listTypeOfExpenses = _listTypeOfExpeses;
    }

    public void AddTotalSumm(double amount)
    {
        TotalSummOfExpenses += amount;
    }

    public void ReduceTotalSumm(double amount)
    {
        TotalSummOfExpenses -= amount;
    }
}