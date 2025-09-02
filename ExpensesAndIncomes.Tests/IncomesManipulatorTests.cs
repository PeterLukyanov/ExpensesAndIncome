using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;

public class IncomesManipulatorTests
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
    public async Task InfoOfIncomes_IncomesExist_ShouldReturnList()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await dbContext.TypesOfIncomes.AddAsync(newTypeOfIncomes);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await dbContext.Incomes.AddAsync(income);

        await dbContext.SaveChangesAsync();

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Incomes for now", result.Error);
    }

    [Fact]
    public async Task AddNewIncomes_IncomeUniqe_ShouldAdd()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await dbContext.TypesOfIncomes.AddAsync(newTypeOfIncomes);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewIncome(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task AddNewIncome_TypeDoesNotFound_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await dbContext.TypesOfIncomes.AddAsync(newTypeOfIncomes);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewIncome(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("This type of Incomes does not found", result.Error);
    }

    [Fact]
    public async Task Update_IncomesIdExist_ShouldUpdate()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await dbContext.TypesOfIncomes.AddRangeAsync(newTypeOfIncomes, newTypeOfIncomes2);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await dbContext.Incomes.AddAsync(income);

        await dbContext.SaveChangesAsync();

        var dto2 = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, income.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Update_IncomeIdDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await dbContext.TypesOfIncomes.AddRangeAsync(newTypeOfIncomes,newTypeOfIncomes2);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await dbContext.Incomes.AddAsync(income);

        await dbContext.SaveChangesAsync();

        var dto2 = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal($"Income whith this Id(9999) does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_IncomeIdExist_ShouldDelete()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await dbContext.TypesOfIncomes.AddRangeAsync(newTypeOfIncomes,newTypeOfIncomes2);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await dbContext.Incomes.AddAsync(income);

        await dbContext.SaveChangesAsync();

        var result = await service.Delete(income.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_IncomeIdDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new IncomesManipulator(dbContext);

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await dbContext.TypesOfIncomes.AddRangeAsync(newTypeOfIncomes,newTypeOfIncomes2);

        await dbContext.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await dbContext.Incomes.AddAsync(income);

        await dbContext.SaveChangesAsync();

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal($"Income whith this Id(99999999) does not exist", result.Error);
    }
}