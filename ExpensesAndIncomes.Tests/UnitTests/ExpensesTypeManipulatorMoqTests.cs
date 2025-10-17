using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;

public class ExpensesTypeManipulatorMoqTests
{
    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseDoesNotExist_ShouldLoad()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        await service.LoadTypeOfExpenses();

        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<TypeOfExpenses>()), Times.Exactly(2));
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseExist_ShouldFail()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        await service.LoadTypeOfExpenses();

        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<TypeOfExpenses>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);
        
        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.InfoTypes();
        
        Assert.True(result.IsFailure);
        Assert.Equal("There are no types of Expenses for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.GetInfoOfType(existTypeOfExpenses[0].Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.GetInfoOfType(9999);
        
        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesExist_ShouldShow()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.TotalSumOfExpenses();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.TotalSumOfExpenses();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Food"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Food", result.Value.Name);
        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<TypeOfExpenses>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Food"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<TypeOfExpenses>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, existTypeOfExpenses[0].Id);

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var dto = new TypeOfExpensesDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.Delete(existTypeOfExpenses[0].Id);

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        expensesRepoMock.Verify(e => e.RemoveRange(It.IsAny<List<Expense>>()), Times.Once);
        expensesTypeRepoMock.Verify(t => t.Remove(It.IsAny<TypeOfExpenses>()), Times.Once);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var existExpenses = new List<Expense>
        {
            new Expense(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<TypeOfExpenses>
        {
            new TypeOfExpenses("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses);

        var result = await service.Delete(9999);

        Assert.True(result.IsFailure);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        expensesRepoMock.Verify(e => e.RemoveRange(It.IsAny<List<Expense>>()), Times.Never);
        expensesTypeRepoMock.Verify(t => t.Remove(It.IsAny<TypeOfExpenses>()), Times.Never);
        Assert.Equal("Such type of Expenses does not exist", result.Error);
    }
    private (ExpensesTypesManipulator service,
             Mock<IExpenseRepository> expensesRepoMock,
             Mock<ITypeOfExpensesRepository> expensesTypeRepoMock,
             Mock<IUnitOfWork> unitMock) Create_Service_ReposMock_UnitMock(List<Expense> existExpenses, List<TypeOfExpenses> existTypeOfExpenses)
    {
        var loggerMock = new Mock<ILogger<ExpensesTypesManipulator>>();

        var expensesRepoMock = new Mock<IExpenseRepository>();
        expensesRepoMock.Setup(r => r.GetAll()).Returns(existExpenses.BuildMock().AsQueryable());

        var expensesTypeRepoMock = new Mock<ITypeOfExpensesRepository>();
        expensesTypeRepoMock.Setup(r => r.GetAll()).Returns(existTypeOfExpenses.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(r => r.expenseRepository).Returns(expensesRepoMock.Object);
        unitMock.Setup(r => r.typeOfExpensesRepository).Returns(expensesTypeRepoMock.Object);

        var service = new ExpensesTypesManipulator(unitMock.Object, loggerMock.Object);
        return (service, expensesRepoMock, expensesTypeRepoMock, unitMock);
    }
}