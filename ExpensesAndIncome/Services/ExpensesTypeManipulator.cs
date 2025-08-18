using System.Text.Json;
using Models;

namespace Services;

public class ExpensesTypesManipulator
{
    public ListTypesOfExpenses listTypesOfExpenses;
    public static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    public static string folderForData = Path.Combine(basePath, "Data");

    public ExpensesTypesManipulator(ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;

        Directory.CreateDirectory(folderForData);
        LoadTypeOfExpenses(folderForData);
    }
    public void AddType(ListOfExpenses listOfExpenses)
    {
        listTypesOfExpenses.listTypeOfExpenses.Add(listOfExpenses);
        string path = Path.Combine(folderForData, "TypesOfExpenses.json");
        string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        path = Path.Combine(folderForData, $"{listOfExpenses.NameOfType}TypeOfExpenses.json");
        json = JsonSerializer.Serialize(listOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    public void LoadTypeOfExpenses(string folderPath)
    {
        string path = Path.Combine(folderPath, "TypesOfExpenses.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<ListTypesOfExpenses>(json)!;
            listTypesOfExpenses.listTypeOfExpenses = loaded.listTypeOfExpenses;
            listTypesOfExpenses.TotalSummOfExpenses = loaded.TotalSummOfExpenses;

            int counter = 0;
            var files = Directory.GetFiles(folderPath, "*TypeOfExpenses.json");
            if (!(files.Length == 0))
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        json = File.ReadAllText(file);
                        listTypesOfExpenses.listTypeOfExpenses[counter] = JsonSerializer.Deserialize<ListOfExpenses>(json)!;
                        counter++;
                    }
                }
            }
        }
        else
        {
            //Initializing some starting type of Expenses;
            ListOfExpenses typeOfExpenses1 = new ListOfExpenses("Food");
            listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses1);
            path = Path.Combine(folderPath, $"{typeOfExpenses1.NameOfType}TypeOfExpenses.json");
            string json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[0], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            ListOfExpenses typeOfExpenses2 = new ListOfExpenses("Relax");
            listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses2);
            path = Path.Combine(folderPath, $"{typeOfExpenses2.NameOfType}TypeOfExpenses.json");
            json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[1], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            path = Path.Combine(folderPath, "TypesOfExpenses.json");
            json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
    public List<ListOfExpenses> InfoTypes()
    {
        return listTypesOfExpenses.listTypeOfExpenses;
    }

    public ListOfExpenses? GetInfoOfType(string type)
    {
        foreach (var listTypeOfExpenses in listTypesOfExpenses.listTypeOfExpenses)
        {
            if (listTypeOfExpenses.NameOfType == type)
                return listTypeOfExpenses;
        }
        return null;
    }
    public double TotalSummOfExpenses()
    {
        return listTypesOfExpenses.TotalSummOfExpenses;
    }

    public void Update(ListOfExpenses listOfExpenses, string nameOfType)
    {
        foreach (var listOfTypes in listTypesOfExpenses.listTypeOfExpenses)
        {
            if (listOfExpenses.NameOfType == listOfTypes.NameOfType)
            {
                listOfTypes.NameOfType = nameOfType;
                string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                string path = Path.Combine(folderForData, "TypesOfExpenses.json");
                File.WriteAllText(path, json);
                foreach (var expense in listOfTypes.listOfExpenses)
                {
                    if (expense.StringTypeOfExpenses == listOfExpenses.NameOfType)
                        expense.StringTypeOfExpenses = nameOfType;
                }

                json = JsonSerializer.Serialize(listOfTypes, new JsonSerializerOptions { WriteIndented = true });
                path = Path.Combine(folderForData, $"{nameOfType}TypeOfExpenses.json");
                File.WriteAllText(path, json);

                path = Path.Combine(folderForData, $"{listOfExpenses.NameOfType}TypeOfExpenses.json");
                File.Delete(path);

                path = Path.Combine(folderForData, "Expenses.json");
                json = File.ReadAllText(path);
                if (File.Exists(path))
                {
                    var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
                    foreach (var expense in expensesList!)
                    {
                        if (expense.StringTypeOfExpenses == listOfExpenses.NameOfType)
                        {
                            expense.StringTypeOfExpenses = nameOfType;
                        }
                    }
                    json = JsonSerializer.Serialize(expensesList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(path, json);
                }
            }
        }
    }

    public void Delete(string nameOfType)
    {
        var listOfExpenses = GetInfoOfType(nameOfType);
        if (listOfExpenses == null)
            return;

        listTypesOfExpenses.TotalSummOfExpenses -= listOfExpenses.TotalSummOfType;
        listTypesOfExpenses.listTypeOfExpenses.Remove(listOfExpenses);

        string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        string path = Path.Combine(folderForData, "TypesOfExpenses.json");
        File.WriteAllText(path, json);

        path = Path.Combine(folderForData, $"{nameOfType}TypeOfExpenses.json");
        File.Delete(path);

        path = Path.Combine(folderForData, "Expenses.json");
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            for (int i = expensesList!.Count-1; i >= 0; i--)
            {
                if (expensesList[i].StringTypeOfExpenses == nameOfType)
                {
                    expensesList.Remove(expensesList[i]);
                }
            }
            json = JsonSerializer.Serialize(expensesList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}