using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using UoW;
using Repositorys;
using Moq;
using Microsoft.Extensions.Logging;

public class ExpensesTypesManipulatorTests
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
        var (service, unit) = CreateServiceAndUnit();

        await service.LoadTypeOfExpenses();

        var result = await unit.typeOfExpensesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseExist_ShouldFail()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        await service.LoadTypeOfExpenses();

        var result = await unit.typeOfExpensesRepository.GetAll().ToListAsync();

        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var (service, unit) = CreateServiceAndUnit();

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no types of Expenses for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType("Food");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var result = await service.GetInfoOfType("Other");

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesExist_ShouldShow()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.TotalSumOfExpenses();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var (service, unit) = CreateServiceAndUnit();

        var result = await service.TotalSumOfExpenses();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var (service, unit) = CreateServiceAndUnit();

        var dto = new TypeOfExpensesDto
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
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        var dto = new TypeOfExpensesDto
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
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, "Food");

        var expense = await unit.expenseRepository.GetAll().FirstAsync(e => e.TypeOfExpenses == "Best Food");
        Assert.True(result.IsSuccess);
        Assert.Equal("Best Food", expense.TypeOfExpenses);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, "Fod");

        var expense = await unit.expenseRepository.GetAll().FirstAsync(e => e.TypeOfExpenses == "Food");

        Assert.True(result.IsFailure);
        Assert.Equal("Food", expense.TypeOfExpenses);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.Delete("Food");

        var typeExist = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Food");
        var expensesExist = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.TypeOfExpenses == "Food");

        Assert.True(result.IsSuccess);
        Assert.Null(typeExist);
        Assert.Null(expensesExist);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var (service, unit) = CreateServiceAndUnit();

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.Delete("Fod");

        var typeExist = await unit.typeOfExpensesRepository.GetAll().FirstOrDefaultAsync(t => t.Name == "Food");
        var expensesExist = await unit.expenseRepository.GetAll().FirstOrDefaultAsync(e => e.TypeOfExpenses == "Food");

        Assert.True(result.IsFailure);
        Assert.NotNull(typeExist);
        Assert.NotNull(expensesExist);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }

    private (ExpensesTypesManipulator service, UnitOfWork unit) CreateServiceAndUnit()
    {
        var loggerMock = new Mock<ILogger<ExpensesTypesManipulator>>();
        var dbContext = GetInMemoryDbContext();
        var repoIncome = new IncomeRepository(dbContext);
        var repoExpense = new ExpenseRepository(dbContext);
        var repoOfTypeIncomes = new TypeOfIncomesRepository(dbContext);
        var repoOfTypeExpenses = new TypeOfExpensesRepository(dbContext);
        var unit = new UnitOfWork(repoExpense, repoIncome, repoOfTypeExpenses, repoOfTypeIncomes, dbContext);
        var service = new ExpensesTypesManipulator(unit, loggerMock.Object);
        return (service,unit);
    }
}