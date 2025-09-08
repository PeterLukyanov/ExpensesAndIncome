using Db;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using UoW;
using Repositorys;
using Moq;
using Microsoft.Extensions.Logging;

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
        var unit = CreateUnit();
        var loggerMock = new Mock<ILogger<TotalSumService>>();
        var service = new TotalSumService(unit, loggerMock.Object);

        TypeOfExpenses newTypeOfExpenses = new TypeOfExpenses("Food");

        await unit.typeOfExpensesRepository.AddAsync(newTypeOfExpenses);

        await unit.SaveChangesAsync();

        Expense newExpense = new Expense(DateTime.Now, 300, "Food", "fsdfsdfdsdfsd");

        await unit.expenseRepository.AddAsync(newExpense);

        await unit.SaveChangesAsync();

        var result = await service.TotalBalance();

        Assert.True(result.IsSuccess);
        Assert.Equal(-300, result.Value);
    }

    [Fact]
    public async Task TotalBalance_ExpensesOrIncomesDoesNotExist_ShouldFail()
    {
        var unit = CreateUnit();
        var loggerMock = new Mock<ILogger<TotalSumService>>();
        var service = new TotalSumService(unit,loggerMock.Object);

        var result = await service.TotalBalance();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expense and incomes", result.Error);
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