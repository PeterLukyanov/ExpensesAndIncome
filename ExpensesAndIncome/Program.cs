using ExpensesAndIncome;
using System.Globalization;
using System.Linq;

// readLine - for value, which made by user
// numberOfMenu - for number of menu, which choose user
string? readLine;
int numberOfMenu;

//Creating instanses of class of types of Expenses and Incomes, that have an arrays in their body;
//Also creating the same instanses for currently Income and Expenses;
ListTypeOfExpenses listTypesOfExpenses = new ListTypeOfExpenses();
ListOfExpenses listOfExpenses = new ListOfExpenses();
ListTypesOfIncomes listTypesOfIncomes = new ListTypesOfIncomes();
ListOfIncomes listOfIncomes = new ListOfIncomes();

//Initializing some starting type of Expenses;
TypeOfExpenses typeOfExpenses1 = new TypeOfExpenses("Food");
TypeOfExpenses typeOfExpenses2 = new TypeOfExpenses("Relax");
listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses1);
listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses2);

//Initializing some starting type of Incomes;
TypeOfIncomes typeOfIncome1 = new TypeOfIncomes("Salary");
TypeOfIncomes typeOfIncome2 = new TypeOfIncomes("Other");
listTypesOfIncomes.listTypeOfIncomes.Add(typeOfIncome1);
listTypesOfIncomes.listTypeOfIncomes.Add(typeOfIncome2);

bool validValue = false;
// Menu start working
do
{
    // Output menu items
    Console.WriteLine("Choose the number of menu(1, 2, 3...), or type \"exit\" to exit: \n");

    Console.WriteLine("1.Show menu of Incomes.");
    Console.WriteLine("2.Show menu of Expenses.");
    Console.WriteLine("3.Show total summ of Income.");
    Console.WriteLine("4.Show total summ of Expenses.");
    Console.WriteLine("5.Show total balance.");

    readLine = Console.ReadLine();


    if (readLine != null)
    {
        validValue = int.TryParse(readLine, out numberOfMenu);
        if (validValue)
        {

            switch (numberOfMenu)
            {
                // Show menu of Incomes.
                case 1:
                    do
                    {
                        // Output Incomes menu items
                        Console.WriteLine("Choose the number of Incomes menu(1, 2, 3...), or type \"exit\" to exit to the previos menu: \n");

                        Console.WriteLine("1.Show types of incomes and total summ of this types.");
                        Console.WriteLine("2.Show incomes list.");
                        Console.WriteLine("3.Write down the income.");
                        Console.WriteLine("4.Write down the type of incomes.");

                        readLine = Console.ReadLine();

                        if (readLine != null)
                        {

                            validValue = int.TryParse(readLine, out numberOfMenu);
                            if (validValue)
                            {
                                switch (numberOfMenu)
                                {
                                    //Show types of incomes and total summ of this types
                                    case 1:
                                        listTypesOfIncomes.Info();
                                        break;

                                    //Show incomes list.
                                    case 2:
                                        listOfIncomes.Info();
                                        break;

                                    //Write down the income
                                    case 3:
                                        listOfIncomes.AddNew(listTypesOfIncomes);
                                        break;

                                    //Write down the type of incomes.
                                    case 4:
                                        listTypesOfIncomes.AddType();
                                        break;

                                    default:
                                        Console.WriteLine("Invalid type, try again!");
                                        break;
                                }
                            }
                        }
                    } while (!(readLine.Trim().ToLower() == "exit"));
                    readLine = "";
                    break;

                // Show menu of Expenses.
                case 2:
                    do
                    {
                        // Output Expenses menu items
                        Console.WriteLine("Choose the number of Expenses menu(1, 2, 3...), or type \"exit\" to exit to the previos menu: \n");

                        Console.WriteLine("1.Show types of expenses and total summ of this types.");
                        Console.WriteLine("2.Show expenses list.");
                        Console.WriteLine("3.Write down the expense.");
                        Console.WriteLine("4.Write down the type of expense.");

                        readLine = Console.ReadLine();

                        if (readLine != null)
                        {

                            validValue = int.TryParse(readLine, out numberOfMenu);
                            if (validValue)
                            {
                                switch (numberOfMenu)
                                {
                                    //Show types of expenses and total summ of this types.
                                    case 1:
                                        listTypesOfExpenses.Info();
                                        break;

                                    //Show expenses list.
                                    case 2:
                                        listOfExpenses.Info();
                                        break;

                                    //Write down the expense.
                                    case 3:
                                        listOfExpenses.AddNew(listTypesOfExpenses);
                                        break;

                                    //Write down the type of expense.
                                    case 4:
                                        listTypesOfExpenses.AddType();
                                        break;

                                    default:
                                        Console.WriteLine("Invalid type, try again!");
                                        break;
                                }
                            }
                        }
                    } while (!(readLine.Trim().ToLower() == "exit"));
                    readLine = "";
                    break;

                //Show total summ of Income
                case 3:
                    Console.WriteLine($"Total summ of Income: {listTypesOfIncomes.TotalSummOfIncomes:C}.");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadKey();
                    break;

                //Show total summ of Expenses
                case 4:
                    Console.WriteLine($"Total summ of Expenses: {listTypesOfExpenses.TotalSummOfExpenses:C}.");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadKey();
                    break;

                //Show total balance.
                case 5:
                    Console.WriteLine($"Total balance is: {listTypesOfIncomes.TotalSummOfIncomes - listTypesOfExpenses.TotalSummOfExpenses:C}");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadKey();
                    break;

                default:
                    Console.WriteLine("Invalid type, try again!");
                    break;
            }
        }

    }

} while (!(readLine.Trim().ToLower() == "exit"));
