using Db;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;

public class TotalSumServiceTests
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
    public async Task TotalBalance_ExpensesOrIncomesAreExists_ShouldShow()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new TotalSummService(dbContext);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await dbContext.TypesOfExpenses.AddAsync(newTypeOfExpenses);

        await dbContext.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfsdfdsdfsd");

        await dbContext.Expenses.AddAsync(newExpense);

        await dbContext.SaveChangesAsync();

        var result = await service.TotalBalance();

        Assert.True(result.IsSuccess);
        Assert.Equal(-300, result.Value);
    }

    [Fact]
    public async Task TotalBalance_ExpensesOrIncomesDoesNotExist_ShouldFail()
    {
        var dbContext = GetInMemoryDbContext();
        var service = new TotalSummService(dbContext);

        var result = await service.TotalBalance();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expense or incomes", result.Error);
    }
}