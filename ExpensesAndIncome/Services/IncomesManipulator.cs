using System;
using System.Text.Json;
using Models;

namespace Services;

public class IncomesManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;

    static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    static string folderForData = Path.Combine(basePath, "Data");

    public IncomesManipulator(ListTypesOfIncomes _listTypesOfIncomes)
    {
        listTypesOfIncomes = _listTypesOfIncomes;
    }
    public List<Income> InfoOfIncomes()
    {
        string path = Path.Combine(folderForData, "Incomes.json");

        if (!File.Exists(path))
        {
            return null;            
        }
        else
        {
            string json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
            return incomesList;
        }
    }

    public void AddNewIncome(Income income)
    {
        List<object> listOfAllIncomes = new List<object>();
        string path = Path.Combine(folderForData, "Incomes.json");
        string json;

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            listOfAllIncomes = JsonSerializer.Deserialize<List<object>>(json)!;
        }
        listOfAllIncomes.Add(income);
        json = JsonSerializer.Serialize(listOfAllIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);


        foreach (var typeOfIncomes in listTypesOfIncomes.listTypeOfIncomes)
        {
            if (typeOfIncomes.NameOfType == income.StringTypeOfIncomes)
            {
                typeOfIncomes.listOfIncomes.Add(income);
                typeOfIncomes.TotalSummOfType += income.Amount;
                listTypesOfIncomes.TotalSummOfIncomes += income.Amount;
                path = Path.Combine(folderForData, $"{income.StringTypeOfIncomes}TypeOfIncomes.json");
                json = JsonSerializer.Serialize(typeOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
        }

        path = Path.Combine(folderForData, "TypesOfIncomes.json");
        json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
        
    }

}