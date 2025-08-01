using System;

namespace ExpensesAndIncome;

public class ArrayTypeOfIncome
{
    public int Index { get; set; }
    public int TotalSumm { get; set; }
    public TypeOfIncome[] typeOfIncomesArray = new TypeOfIncome[] { };

    public ArrayTypeOfIncome()
    {
        Index = -1;
        TotalSumm = 0;
        typeOfIncomesArray = new TypeOfIncome[10];
    }

    public void AddType()
    {
        Console.WriteLine("Type the name of new Income type");
        string? readLine = Console.ReadLine();

        if (readLine != null)
        {
            for (int i = 0; i < typeOfIncomesArray.Length; i++)
            {
                if (typeOfIncomesArray[i] == null)
                {
                    TypeOfIncome newTypeOfIncome = new TypeOfIncome(readLine);
                    typeOfIncomesArray[i] = newTypeOfIncome;
                    break;
                }
            }
            Index++;
        }
    }
}