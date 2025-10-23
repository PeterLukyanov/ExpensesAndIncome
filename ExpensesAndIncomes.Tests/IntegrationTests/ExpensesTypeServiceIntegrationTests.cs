using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using UoW;
using Repositorys;
using Moq;
using Microsoft.Extensions.Logging;
using Factorys;

public class ExpensesTypesServiceTests
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
    public async Task LoadTypeOfExpenses_DataBaseDoesNotExist_ShouldLoad()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        await service.LoadTypesForStart();

        var result = await unit.typeOfExpensesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseExist_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        await service.LoadTypesForStart();

        var result = await unit.typeOfExpensesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal($"There are no {nameof(NameTypeOfExpenses)} for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType(newTypeOfExpenses.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType(9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of expenses does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesExist_ShouldShow()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Food"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Food", result.Value.Name);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Food"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, newTypeOfExpenses.Id);

        var expense = await unit.expenseRepository.GetAll().FirstAsync(e => e.Type == "Best Food");
        Assert.True(result.IsSuccess);
        Assert.Equal("Best Food", expense.Type);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, 9999);

        var expense = await unit.expenseRepository.GetAll().FirstAsync(e => e.Type == "Food");

        Assert.True(result.IsFailure);
        Assert.Equal("Food", expense.Type);
        Assert.Equal("Such type of expenses does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(newTypeOfExpenses.Id);

        var typeExist = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Food");
        var expensesExist = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.Type == "Food");

        Assert.True(result.IsSuccess);
        Assert.Null(typeExist);
        Assert.Null(expensesExist);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var (service, unit, factoryTypeMock, factoryOperationsMock) = CreateServiceAndUnit();

        NameTypeOfExpenses newTypeOfExpenses = factoryTypeMock.Object.Create("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = factoryOperationsMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.Delete(9999);

        var typeExist = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Food");
        var expensesExist = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.Type == "Food");

        Assert.True(result.IsFailure);
        Assert.NotNull(typeExist);
        Assert.NotNull(expensesExist);
        Assert.Equal("Such type of expenses does not exist", result.Error);
    }

    private (ExpensesTypesService service,
             UnitOfWork unitMock,
            Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>> factoryTypeMock, 
            Mock<IOperationFactory<Expense>> factoryOperationsMock) CreateServiceAndUnit()
    {
        var loggerMock = new Mock<ILogger<ExpensesTypesService>>();
        var dbContext = GetInMemoryDbContext();
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
        var service = new ExpensesTypesService(unit, loggerMock.Object, factoryTypeMock.Object);
        return (service,unit, factoryTypeMock,factoryOperationsMock);
    }
}