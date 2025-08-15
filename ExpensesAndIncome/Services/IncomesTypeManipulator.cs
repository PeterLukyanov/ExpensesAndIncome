using System.Text.Json;
using Models;

namespace Services;

public class IncomesTypeManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;
    static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    static string folderForData = Path.Combine(basePath, "Data");

    public IncomesTypeManipulator(ListTypesOfIncomes _listTypesOfIncomes)
    {
        listTypesOfIncomes = _listTypesOfIncomes;

        Directory.CreateDirectory(folderForData);
        LoadTypeOfIncomes(folderForData);
    }
    public void AddType(ListOfIncomes listOfIncomes)
    {

        listTypesOfIncomes.listTypeOfIncomes.Add(listOfIncomes);
        string path = Path.Combine(folderForData, "TypesOfIncomes.json");
        string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        path = Path.Combine(folderForData, $"{listOfIncomes.NameOfType}TypeOfIncomes.json");
        json = JsonSerializer.Serialize(listOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public void LoadTypeOfIncomes(string folderPath)
    {
        string path = Path.Combine(folderPath, "TypesOfIncomes.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<ListTypesOfIncomes>(json)!;
            listTypesOfIncomes.listTypeOfIncomes = loaded.listTypeOfIncomes;
            listTypesOfIncomes.TotalSummOfIncomes = loaded.TotalSummOfIncomes;

            int counter = 0;
            var files = Directory.GetFiles(folderPath, "*TypeOfIncomes.json");
            if (!(files.Length == 0))
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        json = File.ReadAllText(file);
                        listTypesOfIncomes.listTypeOfIncomes[counter] = JsonSerializer.Deserialize<ListOfIncomes>(json)!;
                        counter++;
                    }
                }
            }
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
        }
    }

    public List<ListOfIncomes> InfoTypes()
    {
        return listTypesOfIncomes.listTypeOfIncomes;
    }

    public ListOfIncomes? GetInfoOfType(string type)
    {
        foreach (var listTypeOfIncomes in listTypesOfIncomes.listTypeOfIncomes)
        {
            if (listTypeOfIncomes.NameOfType == type)
                return listTypeOfIncomes;
        }
        return null;
    }
     public double TotalSummOfIncomes()
    {
        return listTypesOfIncomes.TotalSummOfIncomes;
    }
}