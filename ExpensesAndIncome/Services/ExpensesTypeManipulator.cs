using System.Text.Json;
using Models;
using PathsFile;

namespace Services;

public class ExpensesTypesManipulator
{
    public ListTypesOfExpenses listTypesOfExpenses;
    public ExpensesTypesManipulator(ListTypesOfExpenses _listTypesOfExpenses)
    {
        listTypesOfExpenses = _listTypesOfExpenses;

        Directory.CreateDirectory(Paths.FolderForData);
        LoadTypeOfExpenses();
    }
    public void AddType(ListOfExpenses listOfExpenses)
    {
        listTypesOfExpenses.listTypeOfExpenses.Add(listOfExpenses);
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);

        path = Path.Combine(Paths.FolderForData, $"{listOfExpenses.NameOfType}{Paths.TypeOfExpensesName}");
        json = JsonSerializer.Serialize(listOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    public void LoadTypeOfExpenses()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<ListTypesOfExpenses>(json)!;
            listTypesOfExpenses.UpdateTypesOfExpenses(loaded.listTypeOfExpenses);
            listTypesOfExpenses.AddTotalSumm(loaded.TotalSummOfExpenses);

            int counter = 0;
            var files = Directory.GetFiles(Paths.FolderForData, $"*{Paths.TypeOfExpensesName}");
            if (!(files.Length == 0))
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        json = File.ReadAllText(file);
                        var fileWithType = JsonSerializer.Deserialize<ListOfExpenses>(json);
                        listTypesOfExpenses.listTypeOfExpenses.Add(fileWithType);
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
            path = Path.Combine(Paths.FolderForData, $"{typeOfExpenses1.NameOfType}{Paths.TypeOfExpensesName}");
            string json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[0], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            ListOfExpenses typeOfExpenses2 = new ListOfExpenses("Relax");
            listTypesOfExpenses.listTypeOfExpenses.Add(typeOfExpenses2);
            path = Path.Combine(Paths.FolderForData, $"{typeOfExpenses2.NameOfType}{Paths.TypeOfExpensesName}");
            json = JsonSerializer.Serialize(listTypesOfExpenses.listTypeOfExpenses[1], new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
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
                listOfTypes.UpdateName(nameOfType);
                string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
                File.WriteAllText(path, json);
                foreach (var expense in listOfTypes.listOfExpenses)
                {
                    if (expense.TypeOfExpenses == listOfExpenses.NameOfType)
                        expense.UpdateTypeOfExpenses(nameOfType);
                }

                json = JsonSerializer.Serialize(listOfTypes, new JsonSerializerOptions { WriteIndented = true });
                path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfExpensesName}");
                File.WriteAllText(path, json);

                path = Path.Combine(Paths.FolderForData, $"{listOfExpenses.NameOfType}{Paths.TypeOfExpensesName}");
                File.Delete(path);

                path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);
                json = File.ReadAllText(path);
                if (File.Exists(path))
                {
                    var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
                    foreach (var expense in expensesList!)
                    {
                        if (expense.TypeOfExpenses == listOfExpenses.NameOfType)
                        {
                            expense.UpdateTypeOfExpenses(nameOfType);
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

        listTypesOfExpenses.ReduceTotalSumm(listOfExpenses.TotalSummOfType);
        listTypesOfExpenses.listTypeOfExpenses.Remove(listOfExpenses);

        string json = JsonSerializer.Serialize(listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        File.WriteAllText(path, json);

        path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfExpensesName}");
        File.Delete(path);

        path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            var expensesList = JsonSerializer.Deserialize<List<Expense>>(json);
            for (int i = expensesList!.Count-1; i >= 0; i--)
            {
                if (expensesList[i].TypeOfExpenses == nameOfType)
                {
                    expensesList.Remove(expensesList[i]);
                }
            }
            json = JsonSerializer.Serialize(expensesList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}