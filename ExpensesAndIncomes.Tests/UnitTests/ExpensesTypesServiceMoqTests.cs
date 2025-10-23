using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;
using Factorys;

public class ExpensesTypesManipulatorMoqTests
{
    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseDoesNotExist_ShouldLoad()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        await service.LoadTypesForStart();

        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfExpenses>()), Times.Exactly(2));
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadTypeOfExpenses_DataBaseExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food") 
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        await service.LoadTypesForStart();

        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfExpenses>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);
        
        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.InfoTypes();
        
        Assert.True(result.IsFailure);
        Assert.Equal($"There are no {nameof(NameTypeOfExpenses)} for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.GetInfoOfType(existTypeOfExpenses[0].Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.GetInfoOfType(9999);
        
        Assert.True(result.IsFailure);
        Assert.Equal("Such type of expenses does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesExist_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfExpenses_ExpensesDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no expenses", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>();

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Food"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Food", result.Value.Name);
        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfExpenses>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>();
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Food"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
        expensesTypeRepoMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfExpenses>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
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
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Best Food"
        };

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of expenses does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.Delete(existTypeOfExpenses[0].Id);

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        expensesRepoMock.Verify(e => e.RemoveRange(It.IsAny<List<Expense>>()), Times.Once);
        expensesTypeRepoMock.Verify(t => t.Remove(It.IsAny<NameTypeOfExpenses>()), Times.Once);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existExpenses = new List<Expense>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Food", "fsdfssadfsd")
        };
        var existTypeOfExpenses = new List<NameTypeOfExpenses>()
        {
            factoryTypeMock.Object.Create("Food")
        };

        var (service, expensesRepoMock, expensesTypeRepoMock, unitMock) = Create_Service_ReposMock_UnitMock(existExpenses, existTypeOfExpenses, factoryTypeMock);

        var result = await service.Delete(9999);

        Assert.True(result.IsFailure);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        expensesRepoMock.Verify(e => e.RemoveRange(It.IsAny<List<Expense>>()), Times.Never);
        expensesTypeRepoMock.Verify(t => t.Remove(It.IsAny<NameTypeOfExpenses>()), Times.Never);
        Assert.Equal("Such type of expenses does not exist", result.Error);
    }
    private (ExpensesTypesService service,
             Mock<IOperationRepository<Expense>> expensesRepoMock,
             Mock<ITypeOfOperationRepository<NameTypeOfExpenses>> expensesTypeRepoMock,
             Mock<IUnitOfWork> unitMock) 
             Create_Service_ReposMock_UnitMock(List<Expense> existExpenses,
                                                List<NameTypeOfExpenses> existTypeOfExpenses, Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>> factoryTypeMock)
    {
        var loggerMock = new Mock<ILogger<ExpensesTypesService>>();

        var expensesRepoMock = new Mock<IOperationRepository<Expense>>();
        expensesRepoMock.Setup(r => r.GetAll()).Returns(existExpenses.BuildMock().AsQueryable());

        var expensesTypeRepoMock = new Mock<ITypeOfOperationRepository<NameTypeOfExpenses>>();
        expensesTypeRepoMock.Setup(r => r.GetAll()).Returns(existTypeOfExpenses.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(r => r.expenseRepository).Returns(expensesRepoMock.Object);
        unitMock.Setup(r => r.typeOfExpensesRepository).Returns(expensesTypeRepoMock.Object);

        var service = new ExpensesTypesService(unitMock.Object, loggerMock.Object, factoryTypeMock.Object);
        return (service, expensesRepoMock, expensesTypeRepoMock, unitMock);
    }

    private (Mock<IOperationFactory<Expense>> factoryOperationMock,
            Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>> factoryTypeMock)
            Create_type_and_operations_factorys()

    {
        var factoryOperationMock = new Mock<IOperationFactory<Expense>>();
        factoryOperationMock.Setup(f => f.Create(It.IsAny<DateTime>(),It.IsAny<double>(), It.IsAny<string>(),It.IsAny<string>()))
                    .Returns((DateTime dateOfAction, double amount, string type, string comment) =>
                    new Expense(dateOfAction,amount,type,comment));
        var factoryTypeMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfExpenses>>();
        factoryTypeMock.Setup(f => f.Create(It.IsAny<string>()))
                    .Returns((string NameOfType) => new NameTypeOfExpenses(NameOfType));
        return (factoryOperationMock, factoryTypeMock);     
    }
}