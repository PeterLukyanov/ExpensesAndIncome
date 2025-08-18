using System;
using System.Text.Json;
using Models;

namespace Services;

public class ExpensesManipulator
{
    public ListTypesOfExpenses listTypesOfExpenses;
    private static int Count = 0;
    static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    public DateTime dataOfAction { get; private set; }
    static string folderForData = Path.Combine(basePath, "Data");
    public ExpensesManipulator(ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;
    }
    public List<Expense> InfoOfExpenses()
    {
        string path = Path.Combine(folderForData, "Expenses.json");

        if (!File.Exists(path))
            return null!;
        else
        {
            string json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            return expensesList!;
        }
    }

    public void AddNewExpense(Expense expense)
    {
        string path = Path.Combine(folderForData, "Counter.json");
        string json;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            Count = JsonSerializer.Deserialize<int>(json);
        }

        expense.Id = ++Count;
        dataOfAction = DateTime.Now;
        expense.DataOfAction = dataOfAction;

        path = Path.Combine(folderForData, "Counter.json");
        json = JsonSerializer.Serialize(Count);
        File.WriteAllText(path, json);

        List<object> listOfAllExpenses = new List<object>();
        path = Path.Combine(folderForData, "Expenses.json");


        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            listOfAllExpenses = JsonSerializer.Deserialize<List<object>>(json)!;
        }
        listOfAllExpenses.Add(expense);
        json = JsonSerializer.Serialize(listOfAllExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        foreach (var typeOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            if (typeOfExpenses.NameOfType == expense.StringTypeOfExpenses)
            {
                typeOfExpenses.listOfExpenses.Add(expense);
                typeOfExpenses.TotalSummOfType += expense.Amount;
                listTypesOfExpenses.TotalSummOfExpenses += expense.Amount;
                path = Path.Combine(folderForData, $"{expense.StringTypeOfExpenses}TypeOfExpenses.json");
                json = JsonSerializer.Serialize(typeOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
        }
        path = Path.Combine(folderForData, "TypesOfExpenses.json");
        json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public void Delete(int id)
    {
        string path = Path.Combine(folderForData, "Expenses.json");
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
                    listTypesOfExpenses.TotalSummOfExpenses -= expense.Amount;
                    listOfExpenses.TotalSummOfType -= expense.Amount;
                    listOfExpenses.listOfExpenses.Remove(expense);

                    string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
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



