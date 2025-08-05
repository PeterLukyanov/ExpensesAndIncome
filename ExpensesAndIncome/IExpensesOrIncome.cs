using System;

namespace ExpensesAndIncome;

public interface IExpensesOrIncome
{
    public DateTime DataOfAction { get;  }
    public int Amount { get;  }
    public string StringTypeOfExpenses { get; }
    public string Comment { get;  }

}