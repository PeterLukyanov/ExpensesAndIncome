using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;
using UoW;
using Repositorys;
using Moq;
using Microsoft.Extensions.Logging;

public class ExpensesManipulatorTests
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
    public async Task InfoOfExpenses_ExpensesExist_ShouldReturnList()
    {
        var (unit, service) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.InfoOfExpenses();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var (unit, service) = CreateServiceAndUnit();

        var result = await service.InfoOfExpenses();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Expenses for now", result.Error);
    }

    [Fact]
    public async Task AddNewExpense_ExpenseUniqe_ShouldAdd()
    {
        var (unit, service) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewExpense(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task AddNewExpense_TypeDoesNotFound_ShouldFail()
    {
        var (unit, service) = CreateServiceAndUnit();
        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewExpense(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("This type of Expenses was not found", result.Error);
    }

    [Fact]
    public async Task Update_ExpensesIdExist_ShouldUpdate()
    {
        var (unit, service) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var dto2 = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, expense.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Update_ExpensesIdDoesNotExist_ShouldFail()
    {
        var (unit, service) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var dto2 = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto2, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (9999)ID", result.Error);
    }

    [Fact]
    public async Task Delete_ExpenseIdExist_ShouldDelete()
    {
        var (unit, service) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(expense.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_ExpenseIdDoesNotExist_ShouldFail()
    {
        var (unit, service) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        TypeOfExpenses newTypeOfExpenses2 = new TypeOfExpenses("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = new Expense(DateTime.Now, dto.Amount, dto.TypeOfExpenses, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (99999999)ID", result.Error);
    }
    private (UnitOfWork unit, ExpensesManipulator service ) CreateServiceAndUnit()
    {
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<ExpensesManipulator>>();
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var repoUsers = new UserRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext, repoUsers);
        var service = new ExpensesManipulator(unit, loggerMock.Object);
        return (unit, service);
    }
}