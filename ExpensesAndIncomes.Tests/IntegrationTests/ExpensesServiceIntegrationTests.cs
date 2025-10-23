using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;
using UoW;
using Repositorys;
using Moq;
using Microsoft.Extensions.Logging;
using Factorys;

public class ExpensesServiceTests
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
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.InfoOfOperations();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        var result = await service.InfoOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses for now", result.Error);
    }

    [Fact]
    public async Task AddNewExpense_ExpenseUniqe_ShouldAdd()
    {
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task AddNewExpense_TypeDoesNotFound_ShouldFail()
    {
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"{dto.Type} type of expenses was not found", result.Error);
    }

    [Fact]
    public async Task Update_ExpensesIdExist_ShouldUpdate()
    {
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        NameTypeOfExpenses newTypeOfExpenses2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);

        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var dto2 = new OperationDto
        {
            Type = "Other",
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
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        NameTypeOfExpenses newTypeOfExpenses2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var dto2 = new OperationDto
        {
            Type = "Other",
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
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        NameTypeOfExpenses newTypeOfExpenses2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(expense.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_ExpenseIdDoesNotExist_ShouldFail()
    {
        var (unit, service, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        NameTypeOfExpenses newTypeOfExpenses2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);
        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Expense expense = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.expenseRepository.AddAsync(expense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (99999999)ID", result.Error);
    }
    private (UnitOfWork unit, ExpensesService service, Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>> factoryTypeMock, Mock<IOperationFactory<Expense>> factoryOperationsMock ) CreateServiceAndUnit()
    {
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<ExpensesService>>();
        var factoryOperationsMock = new Mock<IOperationFactory<Expense>>();
        factoryOperationsMock.Setup(f => f.Create(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns((DateTime dateOfAction, double amount, string type, string comment) => new Expense(dateOfAction, amount, type, comment));
        var factoryTypeMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>>();
        factoryTypeMock.Setup(f => f.Create(It.IsAny<string>())).Returns((string nameOfType) => new NameTypeOfExpenses(nameOfType));
        var repoIncome = new IncomeRepository(dbContext); 
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var repoUsers = new UserRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext, repoUsers);
        var service = new ExpensesService(unit, loggerMock.Object, factoryOperationsMock.Object);
        return (unit, service, factoryTypeMock, factoryOperationsMock);
    }
}