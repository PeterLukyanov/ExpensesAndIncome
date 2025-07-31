using System;

namespace ExpensesAndIncome;

public class TypeOfExpenses : ITypeOfExpensesAndIncome
{
    public string NameOfType { get; private set; }
    public int TotalSumm { get; private set; }

    public TypeOfExpenses(string nameOfType, int totalSumm)
    {
        NameOfType = nameOfType;
        TotalSumm = totalSumm;
    }
}