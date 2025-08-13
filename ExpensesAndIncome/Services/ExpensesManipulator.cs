using System;
using System.Text.Json;
using Models;

namespace Services;

public static class ExpensesManipulator
{
    public static void LoadExpenses(string folderPath, string fileEnding, ListTypesOfExpenses listTypesOfExpenses)
    {
        int counter = 0;
        var files = Directory.GetFiles(folderPath, $"*{fileEnding}");
        if (!(files.Length == 0))
        {
            foreach (var file in files)
            {
                if (file != null)
                {
                    string json = File.ReadAllText(file);
                    listTypesOfExpenses.listTypeOfExpenses[counter] = JsonSerializer.Deserialize<ListOfExpenses>(json)!;
                    counter++;
                }
            }
        }
    }

    public static void InfoOfExpenses(string folderPath)
    {
        string path = Path.Combine(folderPath, "Expenses.json");

        if (!File.Exists(path))
        {
            Console.WriteLine("Here is no expenses for now.");
            Console.WriteLine("Type eny key to exit.");
            Console.ReadLine();
        }
        else
        {
            string json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            int counter = 0;
            foreach (var expense in expensesList!)
            {
                counter++;
                Console.WriteLine($"{counter}. {expense.Amount:C};\ttype: {expense.StringTypeOfExpenses};\tdate: {expense.DataOfAction};\tcomment: {expense.Comment};");
            }
            Console.WriteLine("Type eny key to exit.");
            Console.ReadLine();
        }
    }

    public static void AddNewExpense(string folderPath, ListTypesOfExpenses listTypesOfExpenses)
    {
        double amount = 0;
        string comment;
        string? readLine;
        int numberOfType = 0;

        do
        {
            Console.WriteLine("Choose type of Expenses:\n");
            for (int i = 0; i < listTypesOfExpenses.listTypeOfExpenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listTypesOfExpenses.listTypeOfExpenses[i].NameOfType};");
            }

            readLine = Console.ReadLine();

            if (readLine != null)
            {

                int.TryParse(readLine, out numberOfType);
                if (!(0 < numberOfType && numberOfType <= listTypesOfExpenses.listTypeOfExpenses.Count))
                {
                    Console.WriteLine("Value is not valid, try again.");
                }

                if (0 < numberOfType && numberOfType <= listTypesOfExpenses.listTypeOfExpenses.Count)
                {
                    bool validValue = false;

                    Console.WriteLine($"You choose {listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1].NameOfType}, enter amount of expenses: ");
                    do
                    {
                        readLine = Console.ReadLine();
                        if (readLine != null)
                        {
                            validValue = double.TryParse(readLine, out amount);
                            if (validValue && amount > 0)
                            {
                                listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1].TotalSummOfType += amount;
                                listTypesOfExpenses.TotalSummOfExpenses += amount;
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

                    listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1].listOfExpenses.Add(new Expense(DateTime.Now, amount, listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1].NameOfType, comment));
                    var expense = new Expense(DateTime.Now, amount, listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1].NameOfType, comment);
                    List<object> listOfAllExpenses = new List<object>();
                    string path = Path.Combine(folderPath, "Expenses.json");
                    string json;

                    if (File.Exists(path))
                    {
                        json = File.ReadAllText(path);
                        listOfAllExpenses = JsonSerializer.Deserialize<List<object>>(json)!;
                    }

                    listOfAllExpenses.Add(expense);
                    json = JsonSerializer.Serialize(listOfAllExpenses, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    path = Path.Combine(folderPath, $"{listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1].NameOfType}TypeOfExpenses.json");
                    json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[numberOfType - 1], new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    path = Path.Combine(folderPath, "TypesOfExpenses.json");
                    json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    Console.WriteLine();
                    break;
                }
            }

        } while (!(readLine?.Trim().ToLower() == "exit"));
    }
}