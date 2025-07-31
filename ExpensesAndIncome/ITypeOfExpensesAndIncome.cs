using System;

namespace ExpensesAndIncome;

public interface ITypeOfExpensesAndIncome
{
    public string NameOfType{ get; }
    public int TotalSum{ get; }
}