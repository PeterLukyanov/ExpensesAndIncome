using System;

namespace ExpensesAndIncome;

public class ArrayTypeOfExpenses
{
    public int Index { get;  set; }
    public int TotalSumm{ get; set; }

    public TypeOfExpenses[] TypeOfExpensesArray = new TypeOfExpenses[] { };
    public ArrayTypeOfExpenses()
    {
        TotalSumm = 0;
        Index = -1;
        TypeOfExpensesArray = new TypeOfExpenses[10];
    }

    public void AddType()
    {
        Console.WriteLine("Type the name of new Expense type");
        string? readLine = Console.ReadLine();

        if (readLine != null)
        {
            for (int i = 0; i < TypeOfExpensesArray.Length; i++)
            {
                if (TypeOfExpensesArray[i] == null)
                {
                    TypeOfExpenses newTypeOfExpense = new TypeOfExpenses(readLine);
                    TypeOfExpensesArray[i] = newTypeOfExpense;
                    break;
                }
            }
            Index++;
        }
}
}