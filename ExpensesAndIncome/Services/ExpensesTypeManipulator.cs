using System.Text.Json;
using Dtos;
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
    }
    public async Task AddType(TypeOfExpensesDto listOfExpensesDto)
    {
        ListOfExpenses listOfExpenses = new ListOfExpenses(listOfExpensesDto.NameOfType);
        listTypesOfExpenses.ListTypeOfExpenses.Add(listOfExpenses);
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs, listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });

        path = Path.Combine(Paths.FolderForData, $"{listOfExpenses.NameOfType}{Paths.TypeOfExpensesName}");
        using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs1,listOfExpenses, new JsonSerializerOptions { WriteIndented = true });
    }
    public void LoadTypeOfExpenses()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        if (File.Exists(path))
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: false);
            var loaded =  JsonSerializer.Deserialize<ListTypesOfExpenses>(fs);
            listTypesOfExpenses.UpdateTypesOfExpenses(loaded.ListTypeOfExpenses);
            listTypesOfExpenses.AddTotalSumm(loaded.TotalSummOfExpenses);
        }
        else
        {
            //Initializing some starting type of Expenses;
            ListOfExpenses typeOfExpenses1 = new ListOfExpenses("Food");
            listTypesOfExpenses.ListTypeOfExpenses.Add(typeOfExpenses1);
            path = Path.Combine(Paths.FolderForData, $"{typeOfExpenses1.NameOfType}{Paths.TypeOfExpensesName}");
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
            JsonSerializer.Serialize(fs, listTypesOfExpenses.ListTypeOfExpenses[0], new JsonSerializerOptions { WriteIndented = true });

            ListOfExpenses typeOfExpenses2 = new ListOfExpenses("Relax");
            listTypesOfExpenses.ListTypeOfExpenses.Add(typeOfExpenses2);
            path = Path.Combine(Paths.FolderForData, $"{typeOfExpenses2.NameOfType}{Paths.TypeOfExpensesName}");
            using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
            JsonSerializer.Serialize(fs1, listTypesOfExpenses.ListTypeOfExpenses[1], new JsonSerializerOptions { WriteIndented = true });

            path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
            using var fs2 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
            JsonSerializer.Serialize(fs2, listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    public List<ListOfExpenses> InfoTypes()
    {
        return listTypesOfExpenses.ListTypeOfExpenses;
    }

    public ListOfExpenses? GetInfoOfType(string type)
    {
        foreach (var listTypeOfExpenses in listTypesOfExpenses.ListTypeOfExpenses)
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

    public async Task Update(TypeOfExpensesDto listOfExpenses, string nameOfType)
    {
        foreach (var listOfTypes in listTypesOfExpenses.ListTypeOfExpenses)
        {
            if (listOfExpenses.NameOfType == listOfTypes.NameOfType)
            {
                listOfTypes.UpdateName(nameOfType);
                string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await JsonSerializer.SerializeAsync(fs, listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
                
                foreach (var expense in listOfTypes.listOfExpenses)
                {
                    if (expense.TypeOfExpenses == listOfExpenses.NameOfType)
                        expense.UpdateTypeOfExpenses(nameOfType);
                }


                path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfExpensesName}");
                using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await JsonSerializer.SerializeAsync(fs1, listOfTypes, new JsonSerializerOptions { WriteIndented = true });
                

                path = Path.Combine(Paths.FolderForData, $"{listOfExpenses.NameOfType}{Paths.TypeOfExpensesName}");
                File.Delete(path);

                path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);
                
                if (File.Exists(path))
                {
                    using var fs2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, useAsync: true);
                    
                        var expensesList = await JsonSerializer.DeserializeAsync<List<Expense>>(fs2);
                    
                        foreach (var expense in expensesList)
                        {
                            if (expense.TypeOfExpenses == listOfExpenses.NameOfType)
                            {
                                expense.UpdateTypeOfExpenses(nameOfType);
                            }
                        }
                    
                    using var fs3 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                    
                        await JsonSerializer.SerializeAsync(fs3, expensesList, new JsonSerializerOptions { WriteIndented = true });
                    
                }
            }
        }
    }

    public async Task Delete(string nameOfType)
    {
        var listOfExpenses = GetInfoOfType(nameOfType);
        if (listOfExpenses == null)
            return;

        listTypesOfExpenses.ReduceTotalSumm(listOfExpenses.TotalSummOfType);
        listTypesOfExpenses.ListTypeOfExpenses.Remove(listOfExpenses);

        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfExpensesName);
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs, listTypesOfExpenses, new JsonSerializerOptions { WriteIndented = true });
        
        path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfExpensesName}");
        File.Delete(path);

        path = Path.Combine(Paths.FolderForData, Paths.ExpensesFileName);
        if (File.Exists(path))
        {
            using var fs2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
            
            var expensesList = await JsonSerializer.DeserializeAsync<List<Expense>>(fs2);
            for (int i = expensesList!.Count-1; i >= 0; i--)
            {
                if (expensesList[i].TypeOfExpenses == nameOfType)
                {
                    expensesList.Remove(expensesList[i]);
                }
            }
            using var fs3 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await JsonSerializer.SerializeAsync(fs3, expensesList, new JsonSerializerOptions { WriteIndented = true });            
        }
    }
}