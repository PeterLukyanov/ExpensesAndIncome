using System;

namespace ExpensesAndIncome;

public class TypeOfIncome : ITypeOfExpensesAndIncome
{
    public string NameOfType{ get; private set; }
    public int TotalSum{ get; private set; }
}