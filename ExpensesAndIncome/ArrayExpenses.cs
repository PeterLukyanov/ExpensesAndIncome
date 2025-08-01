using System;

namespace ExpensesAndIncome;

public class ArrayExpenses
{
    public int Index { get; private set; }
    public int TotalSumm{ get; private set; }
    public Expenses[] expensesArray = new Expenses[] { };

    public ArrayExpenses()
    {
        Index = -1;
        TotalSumm = 0;
        expensesArray = new Expenses[10];
    }

    public void AddExpense(ArrayTypeOfExpenses arrayTypeOfExpenses)
    {
        string dataOfAction;
        int amount;
        string comment;
        string? readLine;
        int numberOfType = 0;
        do
        {
            Console.WriteLine("Choose type of Expenses:\n");
            for (int i = 0; i <= arrayTypeOfExpenses.Index; i++)
            {
                Console.WriteLine($"{i + 1}. {arrayTypeOfExpenses.TypeOfExpensesArray[i].NameOfType};");
            }

            readLine = Console.ReadLine();

            if (readLine != null)
            {

                int.TryParse(readLine, out numberOfType);

                if (0 < numberOfType && numberOfType <= arrayTypeOfExpenses.Index + 1)
                {
                    Console.WriteLine($"You choose {arrayTypeOfExpenses.TypeOfExpensesArray[numberOfType - 1].NameOfType}, enter amount of expenses: ");
                    readLine = Console.ReadLine();
                    if (readLine != null)
                    {
                        int.TryParse(readLine, out amount);
                        arrayTypeOfExpenses.TypeOfExpensesArray[numberOfType - 1].TotalSumm += amount;
                        arrayTypeOfExpenses.TotalSumm += amount;

                        Console.WriteLine("Enter data of current day:");
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
                        expensesArray[Index] = new Expenses(dataOfAction, amount, arrayTypeOfExpenses.TypeOfExpensesArray[numberOfType - 1].NameOfType, comment);
                    }
                }
            }

        }while(!(0 < numberOfType && numberOfType <= arrayTypeOfExpenses.Index + 1));
    }
}