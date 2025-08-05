using System;

namespace ExpensesAndIncome;

public class Expense : IExpensesOrIncome
{
    public DateTime DataOfAction { get; private set; }
    public int Amount { get; private set; }
    public string StringTypeOfExpenses { get; private set; }
    public string Comment { get; private set; }

    public Expense(DateTime dataOfAction, int amount, string stringTypeOfExpenses, string comment)
    {
        DataOfAction = dataOfAction;
        Amount = amount;
        StringTypeOfExpenses = stringTypeOfExpenses;
        Comment = comment;
    }
  
    
}