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
    public List<Expense> InfoOfExpenses()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);

        if (!File.Exists(path))
            return null!;
        else
        {
            string json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            return expensesList!;
        }
    }

    public void AddNewExpense(ExpenseDto dto)
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
            json = File.ReadAllText(path);
            listOfAllExpenses = JsonSerializer.Deserialize<List<Expense>>(json)!;
        }
        listOfAllExpenses.Add(newExpense);
        json = JsonSerializer.Serialize(listOfAllExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        foreach (var typeOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            if (typeOfExpenses.NameOfType == newExpense.TypeOfExpenses)
            {
                typeOfExpenses.listOfExpenses.Add(newExpense);
                typeOfExpenses.AddTotalSummOfType(newExpense.Amount);
                listTypesOfExpenses.AddTotalSumm(newExpense.Amount);
                path = Path.Combine(Paths.FolderForData, $"{newExpense.TypeOfExpenses}{Paths.TypeOfExpensesName}");
                json = JsonSerializer.Serialize(typeOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
        }
        path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public void Delete(int id)
    {
        string path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            for (int i = expensesList!.Count - 1; i >= 0; i--)
            {
                if (expensesList[i].Id == id)
                {
                    expensesList.Remove(expensesList[i]);
                }
            }
            json = JsonSerializer.Serialize(expensesList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        foreach (var listOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            foreach (var expense in listOfExpenses.listOfExpenses)
            {
                if (expense.Id == id)
                {
                    listTypesOfExpenses.ReduceTotalSumm(expense.Amount);
                    listOfExpenses.ReduceTotalSummOfType(expense.Amount);
                    listOfExpenses.listOfExpenses.Remove(expense);

                    string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                    path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
                    File.WriteAllText(path, json);

                    path = Path.Combine(Paths.FolderForData, $"{listOfExpenses.NameOfType}{Paths.TypeOfExpensesName}");
                    json = JsonSerializer.Serialize(listOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);

                    break;
                }
            }
        }


    }
}



