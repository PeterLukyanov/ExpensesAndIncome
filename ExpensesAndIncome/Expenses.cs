using System;

namespace ExpensesAndIncome;

public class Expenses : IExpensesOrIncome
{
    public string DataOfAction { get;  private set; }
    public int Amount { get; private set; }
    public string StringTypeOfExpenses { get; private set; }
    public string Comment { get; private set; }
}