using System;
using System.Text.Json;
using Models;

namespace Services;

public static class ExpensesTypeManipulator
{
    public static void AddType(string folderPath, ListTypesOfExpenses listTypesOfExpenses)
    {
        string? readLine;
        bool validValue = false;
        Console.WriteLine("Type the name of new Expenses type");
        do
        {
            readLine = Console.ReadLine();

            if (readLine == null || readLine == "")
                Console.WriteLine("Invalid name. Try again");
            else
            {
                ListOfExpenses newTypeOfExpense = new ListOfExpenses(readLine);
                listTypesOfExpenses.listTypeOfExpenses.Add(newTypeOfExpense);
                string path = Path.Combine(folderPath, "TypesOfExpenses.json");
                string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);

                path = Path.Combine(folderPath, $"{newTypeOfExpense.NameOfType}TypeOfExpenses.json");
                json = JsonSerializer.Serialize(newTypeOfExpense, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);

                validValue = true;
            }
        } while (!(validValue == true));
    }

    public static ListTypesOfExpenses LoadTypeOfExpenses(string folderPath, ListTypesOfExpenses listTypesOfExpenses)
    {
        string path = Path.Combine(folderPath, "TypesOfExpenses.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            listTypesOfExpenses = JsonSerializer.Deserialize<ListTypesOfExpenses>(json)!;
            path = Path.Combine(folderPath);
            ExpensesManipulator.LoadExpenses(path, "TypeOfExpenses.json", listTypesOfExpenses);
            return listTypesOfExpenses;
        }
        else
        {
            //Initializing some starting type of Expenses;
            ListOfExpenses typeOfExpenses1 = new ListOfExpenses("Food");
            listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses1);
            path = Path.Combine(folderPath, $"{typeOfExpenses1.NameOfType}TypeOfExpenses.json");
            string json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[0], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            ListOfExpenses typeOfExpenses2 = new ListOfExpenses("Relax");
            listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses2);
            path = Path.Combine(folderPath, $"{typeOfExpenses2.NameOfType}TypeOfExpenses.json");
            json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[1], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            path = Path.Combine(folderPath, "TypesOfExpenses.json");
            json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            return listTypesOfExpenses;
        }
    }
    public static void InfoTypes(ListTypesOfExpenses listTypesOfExpenses)
    {
        foreach (var typeOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            Console.WriteLine($"Type: {typeOfExpenses.NameOfType}, total summ: {typeOfExpenses.TotalSummOfType};");
        }
        Console.WriteLine("Type eny key to exit.");
        Console.ReadLine();
    }


}