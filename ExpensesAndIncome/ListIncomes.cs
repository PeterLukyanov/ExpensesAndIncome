using System;

namespace ExpensesAndIncome;

public class ListOfIncomes
{
    public int TotalSummOfType { get; set; }
    public List<Income> listOfIncomes = new List<Income>();

    public ListOfIncomes()
    {
        TotalSummOfType = 0;
        listOfIncomes = new List<Income>();
    }

    public void AddNew(ListTypesOfIncomes listTypesOfIncomes)
    {
        int amount = 0;
        string comment;
        string? readLine;
        int numberOfType = 0;
        do
        {
            Console.WriteLine("Choose type of Income:\n");
            for (int i = 0; i < listTypesOfIncomes.listTypeOfIncomes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listTypesOfIncomes.listTypeOfIncomes[i].NameOfType};");
            }

            readLine = Console.ReadLine();

            if (readLine != null)
            {
                int.TryParse(readLine, out numberOfType);

                if (0 < numberOfType && numberOfType <= listTypesOfIncomes.listTypeOfIncomes.Count + 1)
                {
                    bool validValue = false;

                    Console.WriteLine($"You choose {listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].NameOfType}, enter amount of expends: ");
                    do
                    {
                        readLine = Console.ReadLine();
                        if (readLine != null)
                        {
                            validValue = int.TryParse(readLine, out amount);
                            if (validValue && amount > 0)
                            {
                                listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].TotalSummOfType += amount;
                                listTypesOfIncomes.TotalSummOfIncomes += amount;
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
                    listOfIncomes.Add(new Income(DateTime.Now, amount, listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].NameOfType, comment));
                }

            }

        } while (!(0 < numberOfType && numberOfType <= listTypesOfIncomes.listTypeOfIncomes.Count + 1));
    }
}