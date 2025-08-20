using System.Text.Json;
using System.Threading.Tasks;
using Dtos;
using Models;
using PathsFile;

namespace Services;

public class IncomesTypeManipulator
{
    public ListTypesOfIncomes listTypesOfIncomes;
    public IncomesTypeManipulator(ListTypesOfIncomes _listTypesOfIncomes)
    {
        listTypesOfIncomes = _listTypesOfIncomes;

        Directory.CreateDirectory(Paths.FolderForData);
    }
    public async Task AddType(TypeOfIncomesDto listOfIncomesDto)
    {
        ListOfIncomes listOfIncomes = new ListOfIncomes(listOfIncomesDto.NameOfType);
        listTypesOfIncomes.ListTypeOfIncomes.Add(listOfIncomes);
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs, listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });

        path = Path.Combine(Paths.FolderForData, $"{listOfIncomes.NameOfType}{Paths.TypeOfIncomesName}");
        using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs1, listOfIncomes, new JsonSerializerOptions { WriteIndented = true });
    }

    public void LoadTypeOfIncomes()
    {
        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: false);
            var loaded = JsonSerializer.Deserialize<ListTypesOfIncomes>(fs);
            listTypesOfIncomes.UpdateTypesOfIncomes(loaded!.ListTypeOfIncomes);
            listTypesOfIncomes.AddTotalSumm(loaded.TotalSummOfIncomes);
        }
        else
        {
            //Initializing some starting type of Incomes;
            ListOfIncomes typeOfIncome1 = new ListOfIncomes("Salary");
            listTypesOfIncomes.ListTypeOfIncomes.Add(typeOfIncome1);
            path = Path.Combine(Paths.FolderForData, $"{typeOfIncome1.NameOfType}{Paths.TypeOfIncomesName}");
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
            JsonSerializer.Serialize(fs, listTypesOfIncomes.ListTypeOfIncomes[0], new JsonSerializerOptions { WriteIndented = true });

            ListOfIncomes typeOfIncome2 = new ListOfIncomes("Other");
            listTypesOfIncomes.ListTypeOfIncomes.Add(typeOfIncome2);
            path = Path.Combine(Paths.FolderForData, $"{typeOfIncome2.NameOfType}{Paths.TypeOfIncomesName}");
            using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
            JsonSerializer.Serialize(fs1, listTypesOfIncomes.ListTypeOfIncomes[1], new JsonSerializerOptions { WriteIndented = true });

            path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
            using var fs2 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
            JsonSerializer.Serialize(fs2, listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    public List<ListOfIncomes> InfoTypes()
    {
        return listTypesOfIncomes.ListTypeOfIncomes;
    }

    public ListOfIncomes? GetInfoOfType(string type)
    {
        foreach (var listTypeOfIncomes in listTypesOfIncomes.ListTypeOfIncomes)
        {
            if (listTypeOfIncomes.NameOfType == type)
                return listTypeOfIncomes;
        }
        return null;
    }
    public double TotalSummOfIncomes()
    {
        return listTypesOfIncomes.TotalSummOfIncomes;
    }

    public async Task Update(TypeOfIncomesDto listOfIncomes, string nameOfType)
    {
        foreach (var listOfTypes in listTypesOfIncomes.ListTypeOfIncomes)
        {
            if (listOfIncomes.NameOfType == listOfTypes.NameOfType)
            {
                listOfTypes.UpdateName(nameOfType);

                string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await JsonSerializer.SerializeAsync(fs, listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });

                foreach (var income in listOfTypes.listOfIncomes)
                {
                    if (income.TypeOfIncomes == listOfIncomes.NameOfType)
                        income.UpdateTypeOfIncomes(nameOfType);
                }


                path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfIncomesName}");
                using var fs1 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await JsonSerializer.SerializeAsync(fs1, listOfTypes, new JsonSerializerOptions { WriteIndented = true });

                path = Path.Combine(Paths.FolderForData, $"{listOfIncomes.NameOfType}{Paths.TypeOfIncomesName}");
                File.Delete(path);

                path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);

                if (File.Exists(path))
                {
                    using var fs2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, useAsync: true);
                    var incomesList = await JsonSerializer.DeserializeAsync<List<Income>>(fs2);
                    foreach (var income in incomesList!)
                    {
                        if (income.TypeOfIncomes == listOfIncomes.NameOfType)
                        {
                            income.UpdateTypeOfIncomes(nameOfType);
                        }
                    }
                    using var fs3 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
                    await JsonSerializer.SerializeAsync(fs3, incomesList, new JsonSerializerOptions { WriteIndented = true });

                }
            }
        }
    }

    public async Task Delete(string nameOfType)
    {
        var listOfIncomes = GetInfoOfType(nameOfType);
        if (listOfIncomes == null)
            return;

        listTypesOfIncomes.ReduceTotalSumm(listOfIncomes.TotalSummOfType);
        listTypesOfIncomes.ListTypeOfIncomes.Remove(listOfIncomes);

        string path = Path.Combine(Paths.FolderForData, Paths.TypesOfIncomesName);
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(fs, listTypesOfIncomes, new JsonSerializerOptions { WriteIndented = true });

        path = Path.Combine(Paths.FolderForData, $"{nameOfType}{Paths.TypeOfIncomesName}");
        File.Delete(path);

        path = Path.Combine(Paths.FolderForData, Paths.IncomesFileName);
        if (File.Exists(path))
        {
            using var fs2 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, useAsync: true);
 
            var incomesList = await JsonSerializer.DeserializeAsync<List<Income>>(fs2);
            for (int i = incomesList!.Count - 1; i >= 0; i--)
            {
                if (incomesList[i].TypeOfIncomes == nameOfType)
                {
                    incomesList.Remove(incomesList[i]);
                }
            }
            using var fs3 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await JsonSerializer.SerializeAsync(fs3, incomesList, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}