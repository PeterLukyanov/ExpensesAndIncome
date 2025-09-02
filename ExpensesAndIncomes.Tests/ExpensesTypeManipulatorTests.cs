using Db;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

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
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        await service.LoadTypeOfExpenses();

        var result = await dbContext.TypesOfExpenses.ToListAsync();

        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        await service.LoadTypeOfExpenses();

        var result = await dbContext.TypesOfExpenses.ToListAsync();

        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no types of Expenses for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        var result = await service.GetInfoOfType("Food");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        var result = await service.GetInfoOfType("Other");

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesExist_ShouldShow()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await dbContext.Expenses.AddAsync(newExpense);

        await dbContext.SaveChangesAsync();

        var result = await service.TotalSumOfExpenses();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        var result = await service.TotalSumOfExpenses();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

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
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

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
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await dbContext.Expenses.AddAsync(newExpense);

        await dbContext.SaveChangesAsync();

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, "Food");

        var expense = await dbContext.Expenses.FirstAsync(e => e.TypeOfExpenses == "Best Food");
        Assert.True(result.IsSuccess);
        Assert.Equal("Best Food", expense.TypeOfExpenses);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await dbContext.Expenses.AddAsync(newExpense);

        await dbContext.SaveChangesAsync();

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, "Fod");

        var expense = await dbContext.Expenses.FirstAsync(e => e.TypeOfExpenses == "Food");

        Assert.True(result.IsFailure);
        Assert.Equal("Food", expense.TypeOfExpenses);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await dbContext.Expenses.AddAsync(newExpense);

        await dbContext.SaveChangesAsync();

        var result = await service.Delete("Food");

        var typeExist = await dbContext.TypesOfExpenses.FirstOrDefaultAsync(t => t.Name == "Food");
        var expensesExist = await dbContext.Expenses.FirstOrDefaultAsync(e => e.TypeOfExpenses == "Food");

        Assert.True(result.IsSuccess);
        Assert.Null(typeExist);
        Assert.Null(expensesExist);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new ExpensesTypesManipulator(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd");

        await dbContext.Expenses.AddAsync(newExpense);

        await dbContext.SaveChangesAsync();

        var result = await service.Delete("Fod");

        var typeExist = await dbContext.TypesOfExpenses.FirstOrDefaultAsync(t => t.Name == "Food");
        var expensesExist = await dbContext.Expenses.FirstOrDefaultAsync(e => e.TypeOfExpenses == "Food");

        Assert.True(result.IsFailure);
        Assert.NotNull(typeExist);
        Assert.NotNull(expensesExist);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }
}