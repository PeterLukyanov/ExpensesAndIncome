using System.Text.Json.Serialization;

namespace Models;

public class ListTypesOfIncomes
{
    public double TotalSummOfIncomes { get; private set; }
    public List<ListOfIncomes> ListTypeOfIncomes { get; private set; }

    [JsonConstructor]
    public ListTypesOfIncomes(double totalSummOfIncomes, List<ListOfIncomes> listTypeOfIncomes)
    {
        TotalSummOfIncomes = totalSummOfIncomes;
        ListTypeOfIncomes = listTypeOfIncomes;
}
    public ListTypesOfIncomes()
    {
        TotalSummOfIncomes = 0;
        ListTypeOfIncomes = new List<ListOfIncomes>();
    }

    public void UpdateTypesOfIncomes(List<ListOfIncomes> listTypeOfIncomes)
    {
        ListTypeOfIncomes = listTypeOfIncomes;
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