using System;

namespace ExpensesAndIncome;

public class TypeOfIncome : ITypeOfExpensesAndIncome
{
    public string NameOfType { get;  set; }
    public int TotalSumm { get;  set; }
    

    public TypeOfIncome(string nameOfType)
    {
        NameOfType = nameOfType;
        TotalSumm = 0;
    }

    
}