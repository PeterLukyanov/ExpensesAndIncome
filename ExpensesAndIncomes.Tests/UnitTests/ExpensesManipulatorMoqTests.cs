using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;

public class ExpensesManipulatorMoqTests
{
    [Fact]
    public async Task InfoOfExpenses_ExpensesExist_ShouldReturnList()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now,300,"Food","dsgdfgdf")
        };

        var existTypeOfExpenses = new List<TypeOfExpenses>();

        var (service, repoExpense, repoOfTypeExpenses, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.InfoOfExpenses();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>();

        var existTypeOfExpenses = new List<TypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.InfoOfExpenses();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Expenses for now", result.Error);
    }

    [Fact]
    public async Task AddNewExpense_ExpenseUniqe_ShouldAdd()
    {
        var existExpenses = new List<Expense>();

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewExpense(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        expensesRepoMock.Verify(e => e.AddAsync(It.IsAny<Expense>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewExpense_TypeDoesNotFound_ShouldFail()
    {
        var existExpenses = new List<Expense>();

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewExpense(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("This type of Expenses was not found", result.Error);
        expensesRepoMock.Verify(e => e.AddAsync(It.IsAny<Expense>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_ExpensesIdExist_ShouldUpdate()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food"),
            new TypeOfExpenses("Other")
        };

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var expense = existExpenses.First(e => e.TypeOfExpenses == "Food");

        var result = await service.Update(dto, expense.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_ExpensesIdDoesNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food"),
            new TypeOfExpenses("Other")
        };

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (9999)ID", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_ExpenseIdExist_ShouldDelete()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food"),
            new TypeOfExpenses("Other")
        };

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var expense = existExpenses.First(e => e.TypeOfExpenses == "Food");

        var result = await service.Delete(expense.Id);

        Assert.True(result.IsSuccess);
        expensesRepoMock.Verify(e => e.Remove(It.IsAny<Expense>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_ExpenseIdDoesNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food"),
            new TypeOfExpenses("Other")
        };

        var dto = new ExpenseDto
        {
            TypeOfExpenses = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (99999999)ID", result.Error);
        expensesRepoMock.Verify(e => e.Remove(It.IsAny<Expense>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private (ExpensesManipulator service,
             Mock<IExpenseRepository> expensesRepoMock,
             Mock<ITypeOfExpensesRepository> expensesTypeRepoMock,
             Mock<IUnitOfWork> unitMock) Create_Service_RepoMocks_UnitMock(List<Expense> existExpenses, List<TypeOfExpenses> existTypeOfExpenses)
    {
        var loggerMock = new Mock<ILogger<ExpensesManipulator>>();

        var expensesRepoMock = new Mock<IExpenseRepository>();
        expensesRepoMock.Setup(r => r.GetAll()).Returns(existExpenses.BuildMock().AsQueryable());

        var expensesTypeRepoMock = new Mock<ITypeOfExpensesRepository>();
        expensesTypeRepoMock.Setup(r => r.GetAll()).Returns(existTypeOfExpenses.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(r => r.expenseRepository).Returns(expensesRepoMock.Object);
        unitMock.Setup(r => r.typeOfExpensesRepository).Returns(expensesTypeRepoMock.Object);

        var service = new ExpensesManipulator(unitMock.Object,loggerMock.Object);
        return (service, expensesRepoMock, expensesTypeRepoMock, unitMock);
    }
}