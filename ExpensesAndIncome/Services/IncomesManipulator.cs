using System;
using System.Text.Json;
using Models;

namespace Services;

public class IncomesManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;
    private static int Count = 0;
    static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    public DateTime dataOfAction { get; private set; }
    static string folderForData = Path.Combine(basePath, "Data");

    public IncomesManipulator(ListTypesOfIncomes _listTypesOfIncomes)
    {
        listTypesOfIncomes = _listTypesOfIncomes;
    }
    public List<Income> InfoOfIncomes()
    {
        string path = Path.Combine(folderForData, "Incomes.json");

        if (!File.Exists(path))
            return null!;
        else
        {
            string json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
            return incomesList!;
        }
    }

    public void AddNewIncome(Income income)
    {
        string path = Path.Combine(folderForData, "Counter.json");
        string json;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            Count = JsonSerializer.Deserialize<int>(json);
        }

        income.Id = ++Count;
        dataOfAction = DateTime.Now;

        path = Path.Combine(folderForData, "Counter.json");
        json = JsonSerializer.Serialize(Count);
        File.WriteAllText(path, json);

        List<object> listOfAllIncomes = new List<object>();
        path = Path.Combine(folderForData, "Incomes.json");

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


    public void Delete(int id)
    {
        string path = Path.Combine(folderForData, "Incomes.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Expense>>(json);
            for (int i = incomesList!.Count - 1; i >= 0; i--)
            {
                if (incomesList[i].Id == id)
                {
                    incomesList.Remove(incomesList[i]);
                }
            }
            json = JsonSerializer.Serialize(incomesList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        foreach (var listOfExpenses in listTypesOfIncomes.listTypeOfIncomes)
        {
            foreach (var expense in listOfExpenses.listOfIncomes)
            {
                if (expense.Id == id)
                {
                    listTypesOfIncomes.TotalSummOfIncomes -= expense.Amount;
                    listOfExpenses.TotalSummOfType -= expense.Amount;
                    listOfExpenses.listOfIncomes.Remove(expense);

                    string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                    path = Path.Combine(folderForData, "TypesOfExpenses.json");
                    File.WriteAllText(path, json);

                    path = Path.Combine(folderForData, $"{listOfExpenses.NameOfType}TypeOfExpenses.json");
                    json = JsonSerializer.Serialize(listOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    break;
                }
            }
        }


    }
}