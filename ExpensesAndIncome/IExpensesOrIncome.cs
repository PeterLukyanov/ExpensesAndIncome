using System;

namespace ExpensesAndIncome;

public interface IExpensesOrIncome
{
    public string DataOfAction { get;  }
    public int Amount { get;  }
    public string StringTypeOfExpenses { get; }
    public string Comment { get;  }

}