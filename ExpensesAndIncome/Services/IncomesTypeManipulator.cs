using System.Text.Json;
using Models;
using PathsFile;

namespace Services;

public class IncomesTypeManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;
    public IncomesTypeManipulator(ListTypesOfIncomes _listTypesOfIncomes)
    {
        listTypesOfIncomes = _listTypesOfIncomes;

        Directory.CreateDirectory(Paths.FolderForData);
        LoadTypeOfIncomes();
    }
    public void AddType(ListOfIncomes listOfIncomes)
    {

        listTypesOfIncomes.listTypeOfIncomes.Add(listOfIncomes);
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        path = Path.Combine(Paths.FolderForData, $"{listOfIncomes.NameOfType}{Paths.TypeOfIncomesName}");
        json = JsonSerializer.Serialize(listOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public void LoadTypeOfIncomes()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<ListTypesOfIncomes>(json)!;
            listTypesOfIncomes.UpdateTypesOfIncomes(loaded.listTypeOfIncomes);
            listTypesOfIncomes.AddTotalSumm(loaded.TotalSummOfIncomes);

            int counter = 0;
            var files = Directory.GetFiles(Paths.FolderForData, $"*{Paths.TypeOfIncomesName}");
            if (!(files.Length == 0))
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        json = File.ReadAllText(file);
                        var fileWithType = JsonSerializer.Deserialize<ListOfIncomes>(json);
                        listTypesOfIncomes.listTypeOfIncomes.Add(fileWithType);
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
            path = Path.Combine(Paths.FolderForData, $"{typeOfIncome1.NameOfType}{Paths.TypeOfIncomesName}");
            string json = JsonSerializer.Serialize(listTypesOfIncomes.listTypeOfIncomes[0], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            ListOfIncomes typeOfIncome2 = new ListOfIncomes("Other");
            listTypesOfIncomes.listTypeOfIncomes.Add(typeOfIncome2);
            path = Path.Combine(Paths.FolderForData, $"{typeOfIncome2.NameOfType}{Paths.TypeOfIncomesName}");
            json = JsonSerializer.Serialize(listTypesOfIncomes.listTypeOfIncomes[1], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
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
                listOfTypes.UpdateName(nameOfType);
                string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
                File.WriteAllText(path, json);
                foreach (var income in listOfTypes.listOfIncomes)
                {
                    if (income.TypeOfIncomes == listOfIncomes.NameOfType)
                        income.UpdateTypeOfIncomes(nameOfType);
                }

                json = JsonSerializer.Serialize(listOfTypes, new JsonSerializerOptions { WriteIndented = true });
                path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfIncomesName}");
                File.WriteAllText(path, json);

                path = Path.Combine(Paths.FolderForData, $"{listOfIncomes.NameOfType}{Paths.TypeOfIncomesName}");
                File.Delete(path);

                path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);
                json = File.ReadAllText(path);
                if (File.Exists(path))
                {
                    var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
                    foreach (var income in incomesList!)
                    {
                        if (income.TypeOfIncomes == listOfIncomes.NameOfType)
                        {
                            income.UpdateTypeOfIncomes(nameOfType);
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

        listTypesOfIncomes.ReduceTotalSumm(listOfIncomes.TotalSummOfType);
        listTypesOfIncomes.listTypeOfIncomes.Remove(listOfIncomes);

        string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        File.WriteAllText(path, json);

        path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfIncomesName}");
        File.Delete(path);

        path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
            for (int i = incomesList!.Count-1; i >= 0; i--)
            {
                if (incomesList[i].TypeOfIncomes == nameOfType)
                {
                    incomesList.Remove(incomesList[i]);
                }
            }
            json = JsonSerializer.Serialize(incomesList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}