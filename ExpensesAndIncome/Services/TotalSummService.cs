using Models;

namespace Services;

public class TotalSummService
{
    public ListTypesOfExpenses listTypesOfExpenses;
    public ListTypesOfIncomes listTypesOfIncomes;

    public TotalSummService(ListTypesOfIncomes _listTypesOfIncomes, ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;
        listTypesOfIncomes = _listTypesOfIncomes;
    }

    public double TotalBalance()
    {
        return listTypesOfIncomes.TotalSummOfIncomes - listTypesOfExpenses.TotalSummOfExpenses;
    }

}