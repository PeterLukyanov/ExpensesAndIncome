using System;

namespace ExpensesAndIncome;

public class ListOfExpenses
{
    public double TotalSumm { get; private set; }
    public List<Expense> listOfExpenses = new List<Expense>();

    public ListOfExpenses()
    {
        TotalSumm = 0;
        listOfExpenses = new List<Expense>();
    }

    public void AddNew(ListTypeOfExpenses listTypeOfExpenses)
    {
        double amount = 0;
        string comment;
        string? readLine;
        int numberOfType = 0;
        do
        {
            Console.WriteLine("Choose type of Expenses:\n");
            for (int i = 0; i < listTypeOfExpenses.listTypeOfExpenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listTypeOfExpenses.listTypeOfExpenses[i].NameOfType};");
            }

            readLine = Console.ReadLine();

            if (readLine != null)
            {

                int.TryParse(readLine, out numberOfType);

                if (0 < numberOfType && numberOfType <= listTypeOfExpenses.listTypeOfExpenses.Count + 1)
                {
                    bool validValue = false;

                    Console.WriteLine($"You choose {listTypeOfExpenses.listTypeOfExpenses[numberOfType - 1].NameOfType}, enter amount of expenses: ");
                    do
                    {
                        readLine = Console.ReadLine();
                        if (readLine != null)
                        {
                            validValue = double.TryParse(readLine, out amount);
                            if (validValue && amount > 0)
                            {
                                listTypeOfExpenses.listTypeOfExpenses[numberOfType - 1].TotalSummOfType += amount;
                                listTypeOfExpenses.TotalSummOfExpenses += amount;
                            }
                            else
                                Console.WriteLine("Not valid amount, try again.");
                        }
                    } while (!(validValue && amount > 0));

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

                    listOfExpenses.Add(new Expense(DateTime.Now, amount, listTypeOfExpenses.listTypeOfExpenses[numberOfType - 1].NameOfType, comment));
                }

            }

        } while (!(0 < numberOfType && numberOfType <= listTypeOfExpenses.listTypeOfExpenses.Count + 1));
    }
    public void Info()
    {
        if (listOfExpenses.Count == 0)
        {
            Console.WriteLine("Here is no expenses for now.");
            Console.WriteLine("Type eny key to exit.");
            Console.ReadKey();
        }
        else
        {
            int counter = 0;
            foreach (var expense in listOfExpenses)
            {
                counter++;
                Console.WriteLine($"{counter}. {expense.Amount:C},\ttype: {expense.StringTypeOfExpenses},\tdate: {expense.DataOfAction},\tcomment: {expense.Comment};");
            }
            Console.WriteLine("Type eny key to exit.");
            Console.ReadKey();
        }
    }
}