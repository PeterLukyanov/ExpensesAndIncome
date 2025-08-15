using System;
using System.Text.Json;
using Models;

namespace Services;

public class ExpensesManipulator
{
    public ListTypesOfExpenses listTypesOfExpenses;
    static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    static string folderForData = Path.Combine(basePath, "Data");
    public ExpensesManipulator(ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;
    }
    public List<Expense> InfoOfExpenses()
    {
        string path = Path.Combine(folderForData, "Expenses.json");

        if (!File.Exists(path))
            return null;
        else
        {
            string json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            return expensesList;
        }
    }

    public void AddNewExpense(Expense expense)
    {

        List<object> listOfAllExpenses = new List<object>();
        string path = Path.Combine(folderForData, "Expenses.json");
        string json;

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
}

        
    
