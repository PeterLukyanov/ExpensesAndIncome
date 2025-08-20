using System.Text.Json;
using Dtos;
using Models;
using PathsFile;

namespace Services;

public class ExpensesManipulator
{
    public ListTypesOfExpenses listTypesOfExpenses;
    private static int Count = 0;

    public ExpensesManipulator(ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;
    }
    public async Task<List<Expense>> InfoOfExpenses()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);

        if (!File.Exists(path))
            return null!;
        else
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            var expensesList = await JsonSerializer.DeserializeAsync<List<Expense>>(fs);
            return expensesList!;
        }
    }

    public async Task AddNewExpense(ExpenseDto dto)
    {
        string path = Path.Combine(Paths.FolderForData, Paths.CounterFileName);
        string json;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            Count = JsonSerializer.Deserialize<int>(json);
        }
        Expense newExpense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment, ++Count);

        path = Path.Combine(Paths.FolderForData, Paths.CounterFileName);
        json = JsonSerializer.Serialize(Count);
        File.WriteAllText(path, json);

        List<Expense> listOfAllExpenses = new List<Expense>();
        path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);

        if (File.Exists(path))
        {
            using var fs1 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            listOfAllExpenses = await JsonSerializer.DeserializeAsync<List<Expense>>(fs1);
        }
        listOfAllExpenses!.Add(newExpense);
        using var fs2 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs2, listOfAllExpenses, new JsonSerializerOptions { WriteIndented = true });

        foreach (var typeOfExpenses in listTypesOfExpenses.ListTypeOfExpenses)
        {
            if (typeOfExpenses.NameOfType == newExpense.TypeOfExpenses)
            {
                typeOfExpenses.listOfExpenses.Add(newExpense);
                typeOfExpenses.AddTotalSummOfType(newExpense.Amount);
                listTypesOfExpenses.AddTotalSumm(newExpense.Amount);
                path = Path.Combine(Paths.FolderForData, $"{newExpense.TypeOfExpenses}{Paths.TypeOfExpensesName}");
                using var fs3 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
                await JsonSerializer.SerializeAsync(fs3, typeOfExpenses, new JsonSerializerOptions { WriteIndented = true });
            }
        }
        path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        using var fs4 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs4, listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task Delete(int id)
    {
        string path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);
        if (File.Exists(path))
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            var expensesList = await JsonSerializer.DeserializeAsync<List<Expense>>(fs);
            for (int i = expensesList!.Count - 1; i >= 0; i--)
            {
                if (expensesList[i].Id == id)
                {
                    expensesList.Remove(expensesList[i]);
                }
            }
            using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await JsonSerializer.SerializeAsync(fs1, expensesList, new JsonSerializerOptions { WriteIndented = true });
        }

        foreach (var listOfExpenses in listTypesOfExpenses.ListTypeOfExpenses)
        {
            foreach (var expense in listOfExpenses.listOfExpenses)
            {
                if (expense.Id == id)
                {
                    listTypesOfExpenses.ReduceTotalSumm(expense.Amount);
                    listOfExpenses.ReduceTotalSummOfType(expense.Amount);
                    listOfExpenses.listOfExpenses.Remove(expense);

                    path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
                    using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                    await JsonSerializer.SerializeAsync(fs, listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });

                    path = Path.Combine(Paths.FolderForData, $"{listOfExpenses.NameOfType}{Paths.TypeOfExpensesName}");
                    using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                    await JsonSerializer.SerializeAsync(fs1, listOfExpenses, new JsonSerializerOptions { WriteIndented = true });

                    break;
                }
            }
        }
    }
}



