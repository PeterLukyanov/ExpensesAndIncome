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
    public async Task<List<Income>> InfoOfIncomes()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);

        if (!File.Exists(path))
            return null!;
        else
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            var incomesList = await JsonSerializer.DeserializeAsync<List<Income>>(fs);
            return incomesList!;
        }
    }

    public async Task AddNewIncome(IncomeDto dto)
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

        List<Income> listOfAllIncomes = new List<Income>();
        path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);

        if (File.Exists(path))
        {
            using var fs1 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            listOfAllIncomes = await JsonSerializer.DeserializeAsync<List<Income>>(fs1);
        }
        listOfAllIncomes!.Add(newIncome);
        using var fs2 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs2, listOfAllIncomes, new JsonSerializerOptions { WriteIndented = true });



        foreach (var typeOfIncomes in listTypesOfIncomes.ListTypeOfIncomes)
        {
            if (typeOfIncomes.NameOfType == newIncome.TypeOfIncomes)
            {
                typeOfIncomes.listOfIncomes.Add(newIncome);
                typeOfIncomes.AddTotalSummOfType(newIncome.Amount);
                listTypesOfIncomes.AddTotalSumm(newIncome.Amount);
                path = Path.Combine(Paths.FolderForData, $"{newIncome.TypeOfIncomes}{Paths.TypeOfIncomesName}");
                using var fs3 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await JsonSerializer.SerializeAsync(fs3, typeOfIncomes, new JsonSerializerOptions { WriteIndented = true });
            }
        }

        path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        using var fs4 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs4, listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
    }


    public async Task Delete(int id)
    {
        string path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);
        if (File.Exists(path))
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            var incomesList = await JsonSerializer.DeserializeAsync<List<Expense>>(fs);
            for (int i = incomesList!.Count - 1; i >= 0; i--)
            {
                if (incomesList[i].Id == id)
                {
                    incomesList.Remove(incomesList[i]);
                }
            }
            using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await JsonSerializer.SerializeAsync(fs1, incomesList, new JsonSerializerOptions { WriteIndented = true });
        }

        foreach (var listOfIncomes in listTypesOfIncomes.ListTypeOfIncomes)
        {
            foreach (var income in listOfIncomes.listOfIncomes)
            {
                if (income.Id == id)
                {
                    listTypesOfIncomes.ReduceTotalSumm(income.Amount);
                    listOfIncomes.ReduceTotalSummOfType(income.Amount);
                    listOfIncomes.listOfIncomes.Remove(income);

                    path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
                    using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                    await JsonSerializer.SerializeAsync(fs, listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });

                    path = Path.Combine(Paths.FolderForData, $"{listOfIncomes.NameOfType}{Paths.TypeOfExpensesName}");
                    using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                    await JsonSerializer.SerializeAsync(fs1, listOfIncomes, new JsonSerializerOptions { WriteIndented = true });

                    break;
                }
            }
        }
    }
}