using System;

namespace ExpensesAndIncome;

public class ListTypeOfExpenses
{
    public double TotalSummOfExpenses { get; set; }

    public List<TypeOfExpenses> listTypeOfExpenses = new List<TypeOfExpenses>();
    public ListTypeOfExpenses()
    {
        TotalSummOfExpenses = 0;
        listTypeOfExpenses = new List<TypeOfExpenses>();
    }

    public void AddType()
    {
        Console.WriteLine("Type the name of new Expenses type");
        string? readLine = Console.ReadLine();

        if (readLine != null)
        {
            TypeOfExpenses newTypeOfExpense = new TypeOfExpenses(readLine);
            listTypeOfExpenses.Add(newTypeOfExpense);
        }
    }

    public void Info()
    {
        foreach (var typeOfExpenses in listTypeOfExpenses)
        {
            Console.WriteLine($"Type: {typeOfExpenses.NameOfType}, total summ: {typeOfExpenses.TotalSummOfType};");
        }
        Console.WriteLine("Type eny key to exit.");
                    Console.ReadKey();
    }
}