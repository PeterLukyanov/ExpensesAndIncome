using System.Text.Json;
using Models;

namespace Services;

public class ExpensesTypeManipulator
{
    public ListTypesOfExpenses listTypesOfExpenses;
    static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    static string folderForData = Path.Combine(basePath, "Data");

    public ExpensesTypeManipulator(ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;

        Directory.CreateDirectory(folderForData);
        LoadTypeOfExpenses(folderForData);
    }
    public void AddType(ListOfExpenses listOfExpenses)
    {
        listTypesOfExpenses.listTypeOfExpenses.Add(listOfExpenses);
        string path = Path.Combine(folderForData, "TypesOfExpenses.json");
        string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        path = Path.Combine(folderForData, $"{listOfExpenses.NameOfType}TypeOfExpenses.json");
        json = JsonSerializer.Serialize(listOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    public void LoadTypeOfExpenses(string folderPath)
    {
        string path = Path.Combine(folderPath, "TypesOfExpenses.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<ListTypesOfExpenses>(json)!;
            listTypesOfExpenses.listTypeOfExpenses = loaded.listTypeOfExpenses;
            listTypesOfExpenses.TotalSummOfExpenses = loaded.TotalSummOfExpenses;

            int counter = 0;
            var files = Directory.GetFiles(folderPath, "*TypeOfExpenses.json");
            if (!(files.Length == 0))
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        json = File.ReadAllText(file);
                        listTypesOfExpenses.listTypeOfExpenses[counter] = JsonSerializer.Deserialize<ListOfExpenses>(json)!;
                        counter++;
                    }
                }
            }
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
        }
    }
    public List<ListOfExpenses> InfoTypes()
    {
        return listTypesOfExpenses.listTypeOfExpenses;
    }

    public ListOfExpenses? GetInfoOfType(string type)
    {
        foreach (var listTypeOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            if (listTypeOfExpenses.NameOfType == type)
                return listTypeOfExpenses;
        }
        return null;
    }
    public double TotalSummOfExpenses()
    {
        return listTypesOfExpenses.TotalSummOfExpenses;
    }
}