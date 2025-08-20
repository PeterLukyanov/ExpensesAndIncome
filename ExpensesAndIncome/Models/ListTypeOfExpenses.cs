using System.Text.Json.Serialization;

namespace Models;

public class ListTypesOfExpenses
{
    public double TotalSummOfExpenses { get; private set; }

    public List<ListOfExpenses> ListTypeOfExpenses { get; private set; }

    [JsonConstructor]
    public ListTypesOfExpenses(double totalSummOfExpenses, List<ListOfExpenses> listTypeOfExpenses)
    {
        TotalSummOfExpenses = totalSummOfExpenses;
        ListTypeOfExpenses = listTypeOfExpenses;
    }
    public ListTypesOfExpenses()
    {
        TotalSummOfExpenses = 0;
        ListTypeOfExpenses = new List<ListOfExpenses>();
    }

    public void UpdateTypesOfExpenses(List<ListOfExpenses> listTypeOfExpeses)
    {
        ListTypeOfExpenses = listTypeOfExpeses;
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