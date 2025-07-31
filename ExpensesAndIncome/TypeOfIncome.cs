using System;

namespace ExpensesAndIncome;

public class TypeOfIncome : ITypeOfExpensesAndIncome
{
    public string NameOfType { get; private set; }
    public int TotalSumm { get; private set; }

    public TypeOfIncome(string nameOfType, int totalSumm)
    {
        NameOfType = nameOfType;
        TotalSumm = totalSumm;
    }
}