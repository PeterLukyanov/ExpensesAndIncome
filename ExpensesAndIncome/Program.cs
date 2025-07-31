using ExpensesAndIncome;
using System.Globalization;


int totalExpenses = 0;
int totalIncome = 0;

Console.WriteLine("Choose the number of menu(1, 2, 3...), or type \"exit\" to exit: \n");

Console.WriteLine("1.Show total summ of Income.");
Console.WriteLine("2.Show total summ of Expenses.");
Console.WriteLine("3.Show types of Income.");
Console.WriteLine("4.Show types of Expenses.");
Console.WriteLine("5.Write down the expense.");
Console.WriteLine("6.Write down the income.");
Console.WriteLine("7.Show incomes list.");
Console.WriteLine("8.Show expenses list.");

string? readLine;
string numberOfMenu = "";
TypeOfExpenses[] typeOfExpensesArray = new TypeOfExpenses[5];
int indexTypeOfExpensesArray = 0;
TypeOfIncome[] typeOfIncomeArray = new TypeOfIncome[2];
int indexTypeOfIncomeArray = 0;
Expenses[] expensesArray = new Expenses[10];
int indexOfExpensesArray = 0;
Income[] incomesArray = new Income[10];
int indexOfIncomeArray = 0;


TypeOfExpenses typeOfExpenses1 = new TypeOfExpenses("Food", 0);
TypeOfExpenses typeOfExpenses2 = new TypeOfExpenses("Relax", 0);
typeOfExpensesArray[0] = typeOfExpenses1;
typeOfExpensesArray[1] = typeOfExpenses2;
indexTypeOfExpensesArray += 2;

TypeOfIncome typeOfIncome1 = new TypeOfIncome("Salary", 0);
TypeOfIncome typeOfIncome2 = new TypeOfIncome("Other", 0);
typeOfIncomeArray[0] = typeOfIncome1;
typeOfIncomeArray[1] = typeOfIncome2;
indexTypeOfIncomeArray += 2;

do
{
    readLine = Console.ReadLine();
    if (readLine.ToLower() == "exit")
    {
        break;
    }

    if (readLine != null)
    {
        numberOfMenu = readLine;

        switch (numberOfMenu)
        {
            case "1":
                Console.WriteLine($"Total summ of Income: {totalIncome}.");
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            case "2":
                Console.WriteLine($"Total summ of Expenses: {totalExpenses}.");
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            case "3":
                Console.WriteLine("Types of Expenses:\n");
                for (int i = 0; i < indexTypeOfExpensesArray; i++)
                {
                    if (!(typeOfExpensesArray[i].NameOfType == null && typeOfExpensesArray[i].NameOfType == ""))
                        Console.WriteLine(typeOfExpensesArray[i].NameOfType);
                }

                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            case "4":
                Console.WriteLine("Types of Income:\n");
                for (int i = 0; i < indexTypeOfIncomeArray; i++)
                {
                    if (!(typeOfIncomeArray[i].NameOfType == null && typeOfIncomeArray[i].NameOfType == ""))
                        Console.WriteLine(typeOfIncomeArray[i].NameOfType);
                }

                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            case "5":

                int numberOfType = 0;
                int amount = 0;
                string dataOfAction = "";
                string comment = "";

                Console.WriteLine("Choose type of Expenses:\n");
                for (int i = 0; i < indexTypeOfExpensesArray; i++)
                {
                    Console.WriteLine($"{i + 1}. {typeOfExpensesArray[i].NameOfType};");
                }

                readLine = Console.ReadLine();

                if (readLine != null)
                {

                    int.TryParse(readLine, out numberOfType);


                    if (0 < numberOfType && numberOfType <= indexTypeOfExpensesArray)
                    {
                        Console.WriteLine($"You choose {typeOfExpensesArray[numberOfType-1].NameOfType}, enter amount of expenses: ");
                        readLine = Console.ReadLine();
                        if (readLine != null)
                        {
                            int.TryParse(readLine, out amount);
                            totalExpenses += amount;
                            Console.WriteLine("Enter data of current day:");
                            readLine = Console.ReadLine();
                            dataOfAction = readLine;
                            Console.WriteLine("Enter a comment, if you need:");
                            readLine = Console.ReadLine();
                            comment = readLine;
                            expensesArray[indexOfExpensesArray] = new Expenses(dataOfAction, amount, typeOfExpensesArray[numberOfType - 1].NameOfType, comment);
                            indexOfExpensesArray++;
                        }
                    }
                }
                break;

            case "6":
                numberOfType = 0;
                amount = 0;
                dataOfAction = "";
                comment = "";

                Console.WriteLine("Choose type of Incomes\n");
                for (int i = 0; i < indexTypeOfIncomeArray; i++)
                {
                    Console.WriteLine($"{i + 1}. {typeOfIncomeArray[i]};");
                }

                if (0 < numberOfType && numberOfType <= typeOfIncomeArray.Length)
                {
                    Console.WriteLine($"You choose {typeOfIncomeArray[numberOfType-1]}, enter amount of expenses: ");
                    readLine = Console.ReadLine();
                    if (readLine != null)
                    {
                        int.TryParse(readLine, out amount);
                        totalExpenses += amount;
                        Console.WriteLine("Enter data of current day:");
                        readLine = Console.ReadLine();
                        dataOfAction = readLine;
                        Console.WriteLine("Enter a comment, if you need:");
                        incomesArray[indexOfIncomeArray] = new Income(dataOfAction, amount, typeOfIncomeArray[numberOfType].NameOfType, comment);
                        indexOfIncomeArray++;
                    }
                }
                break;
            case "7":
                int counter = 0;
                bool notNullList = false;
                foreach (Income income in incomesArray)
                {
                    if (income != null)
                    {
                        counter++;
                        Console.WriteLine($"{counter}. Amount: {income.Amount:C}, Type of expence: {income.StringTypeOfExpenses}, Data: {income.DataOfAction}, Comment: {income.Comment};");
                        notNullList = true;
                    }
                }
                if (!notNullList)
                    Console.WriteLine("Expence list is empty.");
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

                case "8":
                counter = 0;
                notNullList = false;
                foreach (Expenses expense in expensesArray)
                {
                    if (expense != null)
                    {
                        counter++;
                        Console.WriteLine($"{counter}. Amount: {expense.Amount:C},\nType of expence: {expense.StringTypeOfExpenses},\nData: {expense.DataOfAction},\nComment: {expense.Comment};");
                        notNullList = true;
                    }
                }
                if (!notNullList)
                {
                    Console.WriteLine("Expence list is empty.");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadKey();
                    break;
                }
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            default:
                Console.WriteLine("Invalid type, try again!");
                break;
        }

        Console.WriteLine("Choose the number of menu(1, 2, 3...), or type \"exit\" to exit: \n");

        Console.WriteLine("1.Show total summ of Income.");
        Console.WriteLine("2.Show total summ of Expenses.");
        Console.WriteLine("3.Show types of Income.");
        Console.WriteLine("4.Show types of Expenses.");
        Console.WriteLine("5.Write down the expense.");
        Console.WriteLine("6.Write down the income.");
        Console.WriteLine("7.Show incomes list.");
        Console.WriteLine("8.Show expenses list.");

    }

} while (!(readLine == "exit"));
