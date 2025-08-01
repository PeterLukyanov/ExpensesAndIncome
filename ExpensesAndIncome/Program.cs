using ExpensesAndIncome;
using System.Globalization;

// readLine - for value, which made by user
// numberOfMenu - for number of menu, which choose user
string? readLine;
string numberOfMenu = "";

//Creating instanses of class of types of Expenses and Incomes, that have an arrays in their body;
//Also creating the same instanses for currently Income and Expenses;
ArrayTypeOfExpenses arrayTypeOfExpenses = new ArrayTypeOfExpenses();
ArrayExpenses arrayExpenses = new ArrayExpenses();
ArrayTypeOfIncome arrayTypeOfIncome = new ArrayTypeOfIncome();
ArrayIncome arrayIncome = new ArrayIncome();

//Initializing some starting type of Expenses;
TypeOfExpenses typeOfExpenses1 = new TypeOfExpenses("Food");
TypeOfExpenses typeOfExpenses2 = new TypeOfExpenses("Relax");
arrayTypeOfExpenses.TypeOfExpensesArray[0] = typeOfExpenses1;
arrayTypeOfExpenses.TypeOfExpensesArray[1] = typeOfExpenses2;
arrayTypeOfExpenses.Index += 2;

//Initializing some starting type of Incomes;
TypeOfIncome typeOfIncome1 = new TypeOfIncome("Salary");
TypeOfIncome typeOfIncome2 = new TypeOfIncome("Other");
arrayTypeOfIncome.typeOfIncomesArray[0] = typeOfIncome1;
arrayTypeOfIncome.typeOfIncomesArray[1] = typeOfIncome2;
arrayTypeOfIncome.Index += 2;


// Menu start working
do
{
    // Build menu items
    Console.WriteLine("Choose the number of menu(1, 2, 3...), or type \"exit\" to exit: \n");

    Console.WriteLine("1.Show total summ of Income.");
    Console.WriteLine("2.Show total summ of Expenses.");
    Console.WriteLine("3.Show types of Expenses.");
    Console.WriteLine("4.Show types of Income.");
    Console.WriteLine("5.Write down the Expenses.");
    Console.WriteLine("6.Write down the Income.");
    Console.WriteLine("7.Show incomes list.");
    Console.WriteLine("8.Show expenses list.");

    readLine = Console.ReadLine();
    if (readLine.Trim().ToLower() == "exit")
        break;

    if (readLine != null)
    {
        numberOfMenu = readLine;

        switch (numberOfMenu)
        {
            //Show total summ of Income
            case "1":
                Console.WriteLine($"Total summ of Income: {arrayTypeOfIncome.TotalSumm:C}.");
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            //Show total summ of Expenses
            case "2":
                Console.WriteLine($"Total summ of Expenses: {arrayTypeOfExpenses.TotalSumm:C}.");
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            //Show types of Expenses
            case "3":
                Console.WriteLine("Types of Expenses:\n");
                for (int i = 0; i < arrayTypeOfExpenses.TypeOfExpensesArray.Length; i++)
                {
                    if (!(arrayTypeOfExpenses.TypeOfExpensesArray[i] == null))
                        Console.WriteLine(arrayTypeOfExpenses.TypeOfExpensesArray[i].NameOfType);
                }

                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            //Show types of Income
            case "4":
                Console.WriteLine("Types of Income:\n");
                for (int i = 0; i < arrayTypeOfIncome.typeOfIncomesArray.Length; i++)
                {
                    if (!(arrayTypeOfIncome.typeOfIncomesArray[i] == null))
                        Console.WriteLine(arrayTypeOfIncome.typeOfIncomesArray[i].NameOfType);
                }

                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            //Write down the Expenses
            case "5":
                arrayExpenses.AddExpense(arrayTypeOfExpenses);
                break;

            //Write down the Income
            case "6":
                arrayIncome.AddIncome(arrayTypeOfIncome);
                break;

            //Show incomes list.
            case "7":
                int counter = 0;
                bool notNullList = false;
                foreach (Income income in arrayIncome.incomesArray)
                {
                    if (income != null)
                    {
                        counter++;
                        Console.WriteLine($"{counter}. Amount: {income.Amount:C}\tType of expence: {income.StringTypeOfExpenses}\tData: {income.DataOfAction}\tComment: {income.Comment};");
                        notNullList = true;
                    }
                }
                if (!notNullList)
                    Console.WriteLine("Expence list is empty.");
                Console.WriteLine("Type eny key to exit.");
                Console.ReadKey();
                break;

            //Show expenses list.
            case "8":
                counter = 0;
                notNullList = false;
                foreach (Expenses expense in arrayExpenses.expensesArray)
                {
                    if (expense != null)
                    {
                        counter++;
                        Console.WriteLine($"{counter}. Amount: {expense.Amount:C}\tType of expence: {expense.StringTypeOfExpenses}\tData: {expense.DataOfAction}\tComment: {expense.Comment};");
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
    }

} while (!(readLine == "exit"));
