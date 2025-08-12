using System;

namespace ExpensesAndIncome;

public class TypeOfIncomes : ITypeOfExpensesAndIncome
{
    public string NameOfType { get;  set; }
    public double TotalSummOfType { get;  set; }
    

    public TypeOfIncomes(string nameOfType)
    {
        NameOfType = nameOfType;
        TotalSummOfType = 0;
    }

    
}