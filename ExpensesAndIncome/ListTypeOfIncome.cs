using System;

namespace ExpensesAndIncome;

public class ListTypesOfIncomes
{
    public double TotalSummOfIncomes { get; set; }
    public List<TypeOfIncomes> listTypeOfIncomes = new List<TypeOfIncomes>();

    public ListTypesOfIncomes()
    {
        TotalSummOfIncomes = 0;
        listTypeOfIncomes = new List<TypeOfIncomes>();
    }

    public void AddType()
    {
        Console.WriteLine("Type the name of new Income type");
        string? readLine = Console.ReadLine();

        if (readLine != null)
        {
            TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes(readLine);
            listTypeOfIncomes.Add(newTypeOfIncomes);
        }
    }
    public void Info()
    {
        foreach (var typeOfIncomes in listTypeOfIncomes)
        {
            Console.WriteLine($"Type: {typeOfIncomes.NameOfType}, total summ: {typeOfIncomes.TotalSummOfType};");
        }
        Console.WriteLine("Type eny key to exit.");
                    Console.ReadKey();
    }

}