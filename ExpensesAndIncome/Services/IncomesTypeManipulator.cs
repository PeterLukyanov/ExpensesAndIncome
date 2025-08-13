using System;
using System.Text.Json;
using Models;

namespace Services;

public static class IncomesTypeManipulator
{
    public static void AddType(string folderPath, ListTypesOfIncomes listTypesOfIncomes)
    {
        string? readLine;
        bool validValue = false;
        Console.WriteLine("Type the name of new Income type");
        do
        {
            readLine = Console.ReadLine();

            if (readLine == null||readLine=="")
                Console.WriteLine("Invalid name. Try again");
            else
            {
                ListOfIncomes newTypeOfIncomes = new ListOfIncomes(readLine);
                listTypesOfIncomes.listTypeOfIncomes.Add(newTypeOfIncomes);
                string path = Path.Combine(folderPath, "TypesOfIncomes.json");
                string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);

                path = Path.Combine(folderPath, $"{newTypeOfIncomes.NameOfType}TypeOfIncomes.json");
                json = JsonSerializer.Serialize(newTypeOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);

                validValue = true;
            }
        } while (!(validValue == true));
    }

    public static ListTypesOfIncomes LoadTypeOfIncomes(string folderPath, ListTypesOfIncomes listTypesOfIncomes)
    {
        string path = Path.Combine(folderPath, "TypesOfIncomes.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            listTypesOfIncomes = JsonSerializer.Deserialize<ListTypesOfIncomes>(json)!;
            path = Path.Combine(folderPath);
            IncomesManipulator.LoadIncomes(path, "TypeOfIncomes.json", listTypesOfIncomes);
            return listTypesOfIncomes;
        }
        else
        {
            //Initializing some starting type of Incomes;
            ListOfIncomes typeOfIncome1 = new ListOfIncomes("Salary");
            listTypesOfIncomes.listTypeOfIncomes.Add(typeOfIncome1);
            path = Path.Combine(folderPath, $"{typeOfIncome1.NameOfType}TypeOfIncomes.json");
            string json = JsonSerializer.Serialize(listTypesOfIncomes.listTypeOfIncomes[0], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            ListOfIncomes typeOfIncome2 = new ListOfIncomes("Other");
            listTypesOfIncomes.listTypeOfIncomes.Add(typeOfIncome2);
            path = Path.Combine(folderPath, $"{typeOfIncome2.NameOfType}TypeOfIncomes.json");
            json = JsonSerializer.Serialize(listTypesOfIncomes.listTypeOfIncomes[1], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            path = Path.Combine(folderPath, "TypesOfIncomes.json");
            json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            return listTypesOfIncomes;
        }
    }
    public static void InfoTypes(ListTypesOfIncomes listTypesOfIncomes)
    {
        foreach (var typeOfIncomes in listTypesOfIncomes.listTypeOfIncomes)
        {
            Console.WriteLine($"Type: {typeOfIncomes.NameOfType}, total summ: {typeOfIncomes.TotalSummOfType};");
        }
        Console.WriteLine("Type eny key to exit.");
        Console.ReadLine();
    }
}