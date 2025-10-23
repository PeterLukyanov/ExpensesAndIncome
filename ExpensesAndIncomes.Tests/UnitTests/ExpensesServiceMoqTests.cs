using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;
using Factorys;

public class ExpensesServiceMoqTests
{
    [Fact]
    public async Task InfoOfExpenses_ExpensesExist_ShouldReturnList()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationsMock.Object.Create(DateTime.Now,300,"Food","dsgdfgdf")
        };

        var existTypeOfExpenses = new List<NameTypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var result = await service.InfoOfOperations();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();

        var existTypeOfExpenses = new List<NameTypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var result = await service.InfoOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses for now", result.Error);
    }

    [Fact]
    public async Task AddNewExpense_ExpenseUniqe_ShouldAdd()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var dto = new OperationDto
        {
            Type = "Food",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        expensesRepoMock.Verify(e => e.AddAsync(It.IsAny<Expense>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewExpense_TypeDoesNotFound_ShouldFail()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food")
        };

       var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"{dto.Type} type of expenses was not found", result.Error);
        expensesRepoMock.Verify(e => e.AddAsync(It.IsAny<Expense>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_ExpensesIdExist_ShouldUpdate()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationsMock.Object.Create(DateTime.Now,300,"Food","dsgdfgdf")
        };

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food"),
            factoryTypeMock.Object.Create("Other")
        };

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var expense = existExpenses.First(e => e.Type == "Food");

        var result = await service.Update(dto, expense.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_ExpensesIdDoesNotExist_ShouldFail()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationsMock.Object.Create(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food"),
            factoryTypeMock.Object.Create("Other")
        };

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (9999)ID", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_ExpenseIdExist_ShouldDelete()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationsMock.Object.Create(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food"),
            factoryTypeMock.Object.Create("Other")
        };

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var expense = existExpenses.First(e => e.Type == "Food");

        var result = await service.Delete(expense.Id);

        Assert.True(result.IsSuccess);
        expensesRepoMock.Verify(e => e.Remove(It.IsAny<Expense>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_ExpenseIdDoesNotExist_ShouldFail()
    {
        var (factoryOperationsMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationsMock.Object.Create(DateTime.Now,300,"Food","sadfsdgfasgdf")
        };

        var existTypeOfExpenses = new List<NameTypeOfExpenses>
        {
            factoryTypeMock.Object.Create("Food"),
            factoryTypeMock.Object.Create("Other")
        };

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_RepoMocks_UnitMock(existExpenses, existTypeOfExpenses, factoryOperationsMock);

        var result = await service.Delete(99999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no expense with this (99999999)ID", result.Error);
        expensesRepoMock.Verify(e => e.Remove(It.IsAny<Expense>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private (ExpensesService service,
             Mock<IOperationRepository<Expense>> expensesRepoMock,
             Mock<ITypeOfOperationRepository<NameTypeOfExpenses>> expensesTypeRepoMock,
             Mock<IUnitOfWork> unitMock)
              Create_Service_RepoMocks_UnitMock(List<Expense> existExpenses, List<NameTypeOfExpenses> existTypeOfExpenses, Mock<IOperationFactory<Expense>> factoryOperationsMock)
    {
        var loggerMock = new Mock<ILogger<ExpensesService>>();

        var expensesRepoMock = new Mock<IOperationRepository<Expense>>();
        expensesRepoMock.Setup(r => r.GetAll()).Returns(existExpenses.BuildMock().AsQueryable());

        var expensesTypeRepoMock = new Mock<ITypeOfOperationRepository<NameTypeOfExpenses>>();
        expensesTypeRepoMock.Setup(r => r.GetAll()).Returns(existTypeOfExpenses.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(r => r.expenseRepository).Returns(expensesRepoMock.Object);
        unitMock.Setup(r => r.typeOfExpensesRepository).Returns(expensesTypeRepoMock.Object);

        var service = new ExpensesService(unitMock.Object, loggerMock.Object, factoryOperationsMock.Object);
        return (service, expensesRepoMock, expensesTypeRepoMock, unitMock);
    }
    
    private (Mock<IOperationFactory<Expense>> factoryOperationsMock,
             Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>> factoryTypeMock) Create_type_and_operations_factorys()

    {
        var factoryOperationsMock = new Mock<IOperationFactory<Expense>>();
        factoryOperationsMock.Setup(f => f.Create(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns((DateTime dateOfAction, double amount, string type, string comment) => new Expense(dateOfAction, amount, type, comment));
        var factoryTypeMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>>();
        factoryTypeMock.Setup(f => f.Create(It.IsAny<string>())).Returns((string nameOfType) => new NameTypeOfExpenses(nameOfType));

        return (factoryOperationsMock,factoryTypeMock);
    }

    
        
    
}