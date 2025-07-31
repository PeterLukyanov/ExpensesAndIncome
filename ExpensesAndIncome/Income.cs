using System;

namespace ExpensesAndIncome;

public class Income : IExpensesOrIncome
{
    public string DataOfAction { get;  private set; }
    public int Amount { get; private set; }
    public string StringTypeOfExpenses { get; private set; }
    public string Comment { get; private set; }
}