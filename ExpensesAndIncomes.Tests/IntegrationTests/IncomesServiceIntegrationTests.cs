using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Dtos;
using Models;
using Repositorys;
using UoW;
using Moq;
using Microsoft.Extensions.Logging;
using Factorys;

public class IncomesServiceTests
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
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();
        
        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var result = await service.InfoOfOperations();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        var result = await service.InfoOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes for now", result.Error);
    }

    [Fact]
    public async Task AddNewIncomes_IncomeUniqe_ShouldAdd()
    {
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task AddNewIncome_TypeDoesNotFound_ShouldFail()
    {
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"{dto.Type} type of incomes was not found", result.Error);
    }

    [Fact]
    public async Task Update_IncomesIdExist_ShouldUpdate()
    {
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        NameTypeOfIncomes newTypeOfIncomes2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var dto2 = new OperationDto
        {
            Type = "Other",
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
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        NameTypeOfIncomes newTypeOfIncomes2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var dto2 = new OperationDto
        {
            Type = "Other",
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
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        NameTypeOfIncomes newTypeOfIncomes2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var result = await service.Delete(income.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_IncomeIdDoesNotExist_ShouldFail()
    {
        var (unit,service, factoryTypeMock, factoryOperationsMock) = CreateUnitAndService();

        NameTypeOfIncomes newTypeOfIncomes = factoryTypeMock.Object.Create("Salary");

        NameTypeOfIncomes newTypeOfIncomes2 = factoryTypeMock.Object.Create("Other");

        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes);
        await unit.typeOfIncomesRepository.AddAsync(newTypeOfIncomes2);

        await unit.SaveChangesAsync();

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        Income income = factoryOperationsMock.Object.Create(DateTime.Now, dto.Amount, dto.Type, dto.Comment);
        await unit.incomeRepository.AddAsync(income);

        await unit.SaveChangesAsync();

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no income with this (99999999)ID", result.Error);
    }

    private (UnitOfWork unit, IncomesService service, Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>> factoryTypeMock, Mock<IOperationFactory<Income>> factoryOperationsMock) CreateUnitAndService()
    {
        var dbContext = GetInMemoryDbContext();
        var loggerMock = new Mock<ILogger<IncomesService>>();
        var factoryOperationsMock = new Mock<IOperationFactory<Income>>();
        factoryOperationsMock.Setup(f => f.Create(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns((DateTime dateOfAction, double amount, string type, string comment) => new Income(dateOfAction, amount, type, comment));
        var factoryTypeMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>>();
        factoryTypeMock.Setup(f => f.Create(It.IsAny<string>())).Returns((string name) => new NameTypeOfIncomes(name));
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var repoUsers = new UserRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext, repoUsers);
        var service = new IncomesService(unit, loggerMock.Object, factoryOperationsMock.Object);
        return (unit, service, factoryTypeMock, factoryOperationsMock);
    }
}