using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;
using Repositorys;
using UoW;
using Moq;
using Microsoft.Extensions.Logging;

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
        var (unit,service) = CreateUnitAndService();
        
        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var (unit,service) = CreateUnitAndService();

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Incomes for now", result.Error);
    }

    [Fact]
    public async Task AddNewIncomes_IncomeUniqe_ShouldAdd()
    {
        var (unit,service) = CreateUnitAndService();

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

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
        var (unit,service) = CreateUnitAndService();

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewIncome(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("This type of Incomes was not found", result.Error);
    }

    [Fact]
    public async Task Update_IncomesIdExist_ShouldUpdate()
    {
        var (unit,service) = CreateUnitAndService();

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

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
        var (unit,service) = CreateUnitAndService();

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var dto2 = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no income with this (9999)ID", result.Error);
    }

    [Fact]
    public async Task Delete_IncomeIdExist_ShouldDelete()
    {
        var (unit,service) = CreateUnitAndService();

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var result = await service.Delete(income.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_IncomeIdDoesNotExist_ShouldFail()
    {
        var (unit,service) = CreateUnitAndService();

        TypeOfIncomes newTypeOfIncomes = new TypeOfIncomes("Salary");

        TypeOfIncomes newTypeOfIncomes2 = new TypeOfIncomes("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = new Income(DateTime.Now, dto.Amount, dto.TypeOfIncomes, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no income with this (99999999)ID", result.Error);
    }

    private (UnitOfWork unit, IncomesManipulator service) CreateUnitAndService()
    {
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<IncomesManipulator>>();
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var repoUsers = new UserRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext, repoUsers);
        var service = new IncomesManipulator(unit, loggerMock.Object);
        return (unit, service);
    }
}