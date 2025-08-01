using System;

namespace ExpensesAndIncome;

public class ArrayIncome
{
    public int Index { get; set; }
    public int TotalSumm { get; set; }
    public Income[] incomesArray = new Income[] {  };

    public ArrayIncome()
    {
        Index = -1;
        TotalSumm = 0;
        incomesArray = new Income[10];
    }

    public void AddIncome(ArrayTypeOfIncome arrayTypeOfIncome)
    {
        string dataOfAction;
        int amount;
        string comment;
        string? readLine;
        int numberOfType = 0;
        do
        {
            Console.WriteLine("Choose type of Income:\n");
            for (int i = 0; i <= arrayTypeOfIncome.Index; i++)
            {
                Console.WriteLine($"{i + 1}. {arrayTypeOfIncome.typeOfIncomesArray[i].NameOfType};");
            }

            readLine = Console.ReadLine();

            if (readLine != null)
            {
                int.TryParse(readLine, out numberOfType);

                if (0 < numberOfType && numberOfType <= arrayTypeOfIncome.Index + 1)
                {
                    Console.WriteLine($"You choose {arrayTypeOfIncome.typeOfIncomesArray[numberOfType - 1].NameOfType}, enter amount of expends: ");
                    readLine = Console.ReadLine();
                    if (readLine != null)
                    {
                        int.TryParse(readLine, out amount);
                        arrayTypeOfIncome.typeOfIncomesArray[numberOfType - 1].TotalSumm += amount;
                        arrayTypeOfIncome.TotalSumm += amount;

                        Console.WriteLine("Enter data of current day: ");
                        do
                        {
                            readLine = Console.ReadLine();
                            if (readLine == null || readLine == "")
                            {
                                Console.WriteLine("Enter valid data.");
                            }
                        } while (readLine == null || readLine == "");

                        dataOfAction = readLine;
                        Console.WriteLine("Enter a comment:");
                        do
                        {
                            readLine = Console.ReadLine();
                            if (readLine == null && readLine == "")
                            {
                                Console.WriteLine("Enter valid comment.");
                            }
                        } while (readLine == null || readLine == "");

                        comment = readLine;
                        Index++;
                        incomesArray[Index] = new Income(dataOfAction, amount, arrayTypeOfIncome.typeOfIncomesArray[numberOfType - 1].NameOfType, comment);
                    }
                }
            }
        } while (!(0 < numberOfType && numberOfType <= arrayTypeOfIncome.Index + 1));
    }
}