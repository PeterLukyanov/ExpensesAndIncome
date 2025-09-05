using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;
using Repositorys;
using UoW;
using Microsoft.AspNetCore.Http.HttpResults;

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
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Incomes for now", result.Error);
    }

    [Fact]
    public async Task AddNewIncomes_IncomeUniqe_ShouldAdd()
    {
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        Assert.Equal("This type of Incomes does not found", result.Error);
    }

    [Fact]
    public async Task Update_IncomesIdExist_ShouldUpdate()
    {
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        Assert.Equal($"Income whith this Id(9999) does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_IncomeIdExist_ShouldDelete()
    {
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        var unit = CreateUnit();
        var service = new IncomesManipulator(unit);

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
        Assert.Equal($"Income whith this Id(99999999) does not exist", result.Error);
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