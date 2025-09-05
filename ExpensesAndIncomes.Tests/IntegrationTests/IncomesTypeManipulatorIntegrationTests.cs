using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using Repositorys;
using UoW;

public class IncomesTypesManipulatorTests
{
    private ExpensesAndIncomesDb GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ExpensesAndIncomesDb>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
        var dbContext = new ExpensesAndIncomesDb(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseDoesNotExist_ShouldLoad()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        await service.LoadTypeOfIncomes();

        var result = await unit.typeOfIncomesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        await service.LoadTypeOfIncomes();

        var result = await unit.typeOfIncomesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no types of Incomes for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType("Salary");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType("Other");

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesExist_ShouldShow()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var result = await service.TotalSummOfIncomes();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        var result = await service.TotalSummOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Salary", result.Value.Name);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, "Salary");

        var income = await unit.incomeRepository.GetAll().FirstAsync(e => e.TypeOfIncomes == "My Salary");
        Assert.True(result.IsSuccess);
        Assert.Equal("My Salary", income.TypeOfIncomes);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, "Salar");

        var income = await unit.incomeRepository.GetAll().FirstAsync(e => e.TypeOfIncomes == "Salary");

        Assert.True(result.IsFailure);
        Assert.Equal("Salary", income.TypeOfIncomes);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var result = await service.Delete("Salary");

        var typeExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Salary");
        var incomesExist = await unit.incomeRepository.GetAll().FirstOrDefaultAsync(e => e.TypeOfIncomes == "Salary");

        Assert.True(result.IsSuccess);
        Assert.Null(typeExist);
        Assert.Null(incomesExist);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var service = new IncomesTypeManipulator(unit);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        Income newIncome = new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd");

        await unit.incomeRepository.AddAsync(newIncome);

        await unit.SaveChangesAsync();

        var result = await service.Delete("Salar");

        var typeExist = await unit.typeOfIncomesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Salary");
        var incomesExist = await unit.incomeRepository.GetAll().FirstOrDefaultAsync(e => e.TypeOfIncomes == "Salary");

        Assert.True(result.IsFailure);
        Assert.NotNull(typeExist);
        Assert.NotNull(incomesExist);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
    }
    private UnitOfWork CreateUnit()
    {
        var dbContext = GetInMemoryDbContext();
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext);
        return unit;
    }
}