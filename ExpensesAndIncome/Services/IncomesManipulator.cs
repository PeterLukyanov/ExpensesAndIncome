using System.Text.Json;
using Dtos;
using Models;
using PathsFile;

namespace Services;

public class IncomesManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;
    private static int Count = 0;

    public IncomesManipulator(ListTypesOfIncomes _listTypesOfIncomes)
    {
        listTypesOfIncomes = _listTypesOfIncomes;
    }
    public List<Income> InfoOfIncomes()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);

        if (!File.Exists(path))
            return null!;
        else
        {
            string json = File.ReadAllText(path);
            var incomesList = JsonSerializer.Deserialize<List<Income>>(json);
            return incomesList!;
        }
    }

    public void AddNewIncome(IncomeDto dto)
    {
        string path = Path.Combine(Paths.FolderForData, Paths.CounterFileName);
        string json;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            Count = JsonSerializer.Deserialize<int>(json);
        }

        Income newIncome = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment, ++Count);
        path = Path.Combine(Paths.FolderForData, Paths.CounterFileName);
        json = JsonSerializer.Serialize(Count);
        File.WriteAllText(path, json);

        List<object> listOfAllIncomes = new List<object>();
        path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            listOfAllIncomes = JsonSerializer.Deserialize<List<object>>(json)!;
        }
        listOfAllIncomes.Add(newIncome);
        json = JsonSerializer.Serialize(listOfAllIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);


        foreach (var typeOfIncomes in listTypesOfIncomes.listTypeOfIncomes)
        {
            if (typeOfIncomes.NameOfType == newIncome.TypeOfIncomes)
            {
                typeOfIncomes.listOfIncomes.Add(newIncome);
                typeOfIncomes.AddTotalSummOfType(newIncome.Amount);
                listTypesOfIncomes.AddTotalSumm(newIncome.Amount);
                path = Path.Combine(Paths.FolderForData, $"{newIncome.TypeOfIncomes}{Paths.TypeOfIncomesName}");
                json = JsonSerializer.Serialize(typeOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
        }

        path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }


    public void Delete(int id)
    {
        string path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);
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

        foreach (var listOfIncomes in listTypesOfIncomes.listTypeOfIncomes)
        {
            foreach (var income in listOfIncomes.listOfIncomes)
            {
                if (income.Id == id)
                {
                    listTypesOfIncomes.ReduceTotalSumm(income.Amount);
                    listOfIncomes.ReduceTotalSummOfType(income.Amount);
                    listOfIncomes.listOfIncomes.Remove(income);

                    string json = JsonSerializer.Serialize(listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                    path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
                    File.WriteAllText(path, json);

                    path = Path.Combine(Paths.FolderForData, $"{listOfIncomes.NameOfType}{Paths.TypeOfExpensesName}");
                    json = JsonSerializer.Serialize(listOfIncomes, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    break;
                }
            }
        }
    }
}