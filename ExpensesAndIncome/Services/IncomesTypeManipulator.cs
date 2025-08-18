using System.Text.Json;
using Models;

namespace Services;

public class IncomesTypeManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;
    public static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    public static string folderForData = Path.Combine(basePath, "Data");

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
    
    public void Update(ListOfIncomes listOfIncomes, string nameOfType)
    {
        foreach (var listOfTypes in listTypesOfIncomes.listTypeOfIncomes)
        {
            if (listOfIncomes.NameOfType == listOfTypes.NameOfType)
            {
                listOfTypes.NameOfType = nameOfType;
                string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                string path = Path.Combine(folderForData, "TypesOfIncomes.json");
                File.WriteAllText(path, json);
                foreach (var income in listOfTypes.listOfIncomes)
                {
                    if (income.StringTypeOfIncomes == listOfIncomes.NameOfType)
                        income.StringTypeOfIncomes = nameOfType;
                }

                json = JsonSerializer.Serialize(listOfTypes, new JsonSerializerOptions { WriteIndented = true });
                path = Path.Combine(folderForData, $"{nameOfType}TypeOfIncomes.json");
                File.WriteAllText(path, json);

                path = Path.Combine(folderForData, $"{listOfIncomes.NameOfType}TypeOfIncomes.json");
                File.Delete(path);

                path = Path.Combine(folderForData, "Incomes.json");
                json = File.ReadAllText(path);
                if (File.Exists(path))
                {
                    var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
                    foreach (var income in incomesList!)
                    {
                        if (income.StringTypeOfIncomes == listOfIncomes.NameOfType)
                        {
                            income.StringTypeOfIncomes = nameOfType;
                        }
                    }
                    json = JsonSerializer.Serialize(incomesList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);
                }
            }
        }
    }

    public void Delete(string nameOfType)
    {
        var listOfIncomes = GetInfoOfType(nameOfType);
        if (listOfIncomes == null)
            return;

        listTypesOfIncomes.TotalSummOfIncomes -= listOfIncomes.TotalSummOfType;
        listTypesOfIncomes.listTypeOfIncomes.Remove(listOfIncomes);

        string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        string path = Path.Combine(folderForData, "TypesOfIncomes.json");
        File.WriteAllText(path, json);

        path = Path.Combine(folderForData, $"{nameOfType}TypeOfIncomes.json");
        File.Delete(path);

        path = Path.Combine(folderForData, "Incomes.json");
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
            for (int i = incomesList!.Count-1; i >= 0; i--)
            {
                if (incomesList[i].StringTypeOfIncomes == nameOfType)
                {
                    incomesList.Remove(incomesList[i]);
                }
            }
            json = JsonSerializer.Serialize(incomesList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}