using System;

namespace ExpensesAndIncome;

public class TypeOfExpenses : ITypeOfExpensesAndIncome
{
    public string NameOfType{ get; private set; }
    public int TotalSum{ get; private set; }
}