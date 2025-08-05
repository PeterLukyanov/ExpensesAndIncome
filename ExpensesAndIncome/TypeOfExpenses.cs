using System;

namespace ExpensesAndIncome;

public class TypeOfExpenses : ITypeOfExpensesAndIncome
{

    public string NameOfType { get;  set; }
    public int TotalSummOfType { get;  set; }

    public TypeOfExpenses(string nameOfType)
    {
        NameOfType = nameOfType;
        TotalSummOfType = 0;
    }
}