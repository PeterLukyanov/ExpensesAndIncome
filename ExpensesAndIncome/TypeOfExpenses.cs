using System;

namespace ExpensesAndIncome;

public class TypeOfExpenses : ITypeOfExpensesAndIncome
{

    public string NameOfType { get;  set; }
    public int TotalSumm { get;  set; }

    public TypeOfExpenses(string nameOfType)
    {
        NameOfType = nameOfType;
        TotalSumm = 0;
    }
}