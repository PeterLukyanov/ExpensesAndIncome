using System.Globalization;
using System.Linq;
using System.Text.Json;
using Models;
using Services;


// readLine - for value, which made by user
// numberOfMenu - for number of menu, which choose user
string? readLine;
int numberOfMenu;

// Сreate collections with income types
ListTypesOfExpenses listTypesOfExpenses = new ListTypesOfExpenses();
ListTypesOfIncomes listTypesOfIncomes = new ListTypesOfIncomes();

string basePath = AppDomain.CurrentDomain.BaseDirectory;
string folderForData = Path.Combine(basePath, "Data");
Directory.CreateDirectory(folderForData);

//Load previously created files if they exist, or create new starter files
listTypesOfExpenses =ExpensesTypeManipulator.LoadTypeOfExpenses(folderForData, listTypesOfExpenses);
listTypesOfIncomes=IncomesTypeManipulator.LoadTypeOfIncomes(folderForData, listTypesOfIncomes);


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
                                        IncomesTypeManipulator.InfoTypes(listTypesOfIncomes);
                                        break;

                                    //Show incomes list.
                                    case 2:
                                        IncomesManipulator.InfoOfIncomes();
                                        break;

                                    //Write down the income
                                    case 3:
                                        IncomesManipulator.AddNewIncome(folderForData, listTypesOfIncomes);
                                        break;

                                    //Write down the type of incomes.
                                    case 4:
                                        IncomesTypeManipulator.AddType(folderForData, listTypesOfIncomes);
                                        break;

                                    default:
                                        Console.WriteLine("Invalid type, try again!");
                                        break;
                                }
                            }
                        }
                    } while (!(readLine!.Trim().ToLower() == "exit"));
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
                                        ExpensesTypeManipulator.InfoTypes(listTypesOfExpenses);
                                        break;

                                    //Show expenses list.
                                    case 2:
                                        ExpensesManipulator.InfoOfExpenses(folderForData);
                                        break;

                                    //Write down the expense.
                                    case 3:
                                        ExpensesManipulator.AddNewExpense(folderForData, listTypesOfExpenses);
                                        break;

                                    //Write down the type of expense.
                                    case 4:
                                        ExpensesTypeManipulator.AddType(folderForData, listTypesOfExpenses);
                                        break;

                                    default:
                                        Console.WriteLine("Invalid type, try again!");
                                        break;
                                }
                            }
                        }
                    } while (!(readLine!.Trim().ToLower() == "exit"));
                    readLine = "";
                    break;

                //Show total summ of Income
                case 3:
                    Console.WriteLine($"Total summ of Income: {listTypesOfIncomes.TotalSummOfIncomes:C}.");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadLine();
                    break;

                //Show total summ of Expenses
                case 4:
                    Console.WriteLine($"Total summ of Expenses: {listTypesOfExpenses.TotalSummOfExpenses:C}.");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadLine();
                    break;

                //Show total balance.
                case 5:
                    Console.WriteLine($"Total balance is: {listTypesOfIncomes.TotalSummOfIncomes - listTypesOfExpenses.TotalSummOfExpenses:C}");
                    Console.WriteLine("Type eny key to exit.");
                    Console.ReadLine();
                    break;

                default:
                    Console.WriteLine("Invalid type, try again!");
                    break;
            }
        }

    }

} while (!(readLine!.Trim().ToLower() == "exit"));
