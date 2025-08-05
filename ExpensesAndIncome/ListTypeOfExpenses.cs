using System;

namespace ExpensesAndIncome;

public class ListTypeOfExpenses
{
    public int TotalSummOfExpenses{ get; set; }

    public List<TypeOfExpenses> listTypeOfExpenses = new List<TypeOfExpenses> ();
    public ListTypeOfExpenses()
    {
        TotalSummOfExpenses = 0;
        listTypeOfExpenses = new List<TypeOfExpenses>();
    }

    public void AddType()
    {
        Console.WriteLine("Type the name of new ESxpense type");
        string? readLine = Console.ReadLine();

        if (readLine != null)
        {
            TypeOfExpenses newTypeOfExpense = new TypeOfExpenses(readLine);
            listTypeOfExpenses.Add(newTypeOfExpense);
        }
}
}