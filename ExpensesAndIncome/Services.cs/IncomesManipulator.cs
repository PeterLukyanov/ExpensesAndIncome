using System;
using System.Text.Json;
using Models;

namespace Services;

public static class IncomesManipulator
{
    public static void LoadIncomes(string folderPath, string fileEnding, ListTypesOfIncomes listTypesOfIncomes)
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
                    listTypesOfIncomes.listTypeOfIncomes[counter] = JsonSerializer.Deserialize<ListOfIncomes>(json)!;
                    counter++;
                }
            }
        }
    }

    public static void InfoOfIncomes()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Incomes.json");

        if (!File.Exists(path))
        {
            Console.WriteLine("Here is no incomes for now.");
            Console.WriteLine("Type eny key to exit.");
            Console.ReadLine();
        }
        else
        {
            string json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
            int counter = 0;
            foreach (var income in incomesList!)
            {
                counter++;
                Console.WriteLine($"{counter}. {income.Amount:C};\ttype: {income.StringTypeOfExpenses};\tdate: {income.DataOfAction};\tcomment: {income.Comment};");
            }
            Console.WriteLine("Type eny key to exit.");
            Console.ReadLine();
        }
    }

    public static void AddNewIncome(string folderPath, ListTypesOfIncomes listTypesOfIncomes)
    {
        double amount = 0;
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
                if (!(0 < numberOfType && numberOfType <= listTypesOfIncomes.listTypeOfIncomes.Count))
                {
                    Console.WriteLine("Value is not valid, try again.");
                }

                if (0 < numberOfType && numberOfType <= listTypesOfIncomes.listTypeOfIncomes.Count)
                {
                    bool validValue = false;

                    Console.WriteLine($"You choose {listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].NameOfType}, enter amount of income: ");
                    do
                    {
                        readLine = Console.ReadLine();
                        if (readLine != null)
                        {
                            validValue = double.TryParse(readLine, out amount);
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


                    listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].listOfIncomes.Add(new Income(DateTime.Now, amount, listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].NameOfType, comment));
                    var income = new Income(DateTime.Now, amount, listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].NameOfType, comment);
                    List<object> listOfAllIncomes = new List<object>();
                    string path = Path.Combine(folderPath, "Incomes.json");
                    string json;

                    if (File.Exists(path))
                    {
                        json = File.ReadAllText(path);
                        listOfAllIncomes = JsonSerializer.Deserialize<List<object>>(json)!;
                    }

                    listOfAllIncomes.Add(income);
                    json = JsonSerializer.Serialize(listOfAllIncomes, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    path = Path.Combine(folderPath, $"{listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1].NameOfType}TypeOfIncomes.json");
                    json = JsonSerializer.Serialize(listTypesOfIncomes.listTypeOfIncomes[numberOfType - 1], new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    path = Path.Combine(folderPath, "TypesOfIncomes.json");
                    json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    Console.WriteLine();
                    break;
                }

            }
        } while (!(readLine?.Trim().ToLower() == "exit"));
    }

}