using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;
using Factorys;

public class IncomesTypesServiceMoqTests
{
    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseDoesNotExist_ShouldLoad()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        await service.LoadTypesForStart();

        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfIncomes>()), Times.Exactly(2));
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        await service.LoadTypesForStart();

        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfIncomes>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal($"There are no {nameof(NameTypeOfIncomes)} for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.GetInfoOfType(existTypeOfIncomes[0].Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.GetInfoOfType(9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of incomes does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesExist_ShouldShow()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.TotalSumOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Salary", result.Value.Name);
        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfIncomes>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<NameTypeOfIncomes>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, existTypeOfIncomes[0].Id);

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var dto = new NameTypeOfOperationsDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of incomes does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.Delete(existTypeOfIncomes[0].Id);

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        repoOfTypeIncomesMock.Verify(t => t.Remove(It.IsAny<NameTypeOfIncomes>()), Times.Once);
        repoIncomeMock.Verify(i => i.RemoveRange(It.IsAny<List<Income>>()), Times.Once);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            factoryOperationMock.Object.Create(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes, factoryTypeMock);

        var result = await service.Delete(9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of incomes does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        repoOfTypeIncomesMock.Verify(t => t.Remove(It.IsAny<NameTypeOfIncomes>()), Times.Never);
        repoIncomeMock.Verify(i => i.RemoveRange(It.IsAny<List<Income>>()), Times.Never);
    }

    private (IncomesTypesService service,
            Mock<IOperationRepository<Income>> repoIncomeMock,
            Mock<ITypeOfOperationRepository<NameTypeOfIncomes>> repoOfTypeIncomesMock,
            Mock<IUnitOfWork> unitMock)
             CreateService(List<Income> existIncomes,
                         List<NameTypeOfIncomes> existTypeOfIncomes,
                         Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>> factoryTypeMock)
    {
        var loggerMock = new Mock<ILogger<IncomesTypesService>>();
        var repoIncomeMock = new Mock<IOperationRepository<Income>>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes.BuildMock().AsQueryable());

        var repoOfTypeIncomesMock = new Mock<ITypeOfOperationRepository<NameTypeOfIncomes>>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypesService(unitMock.Object, loggerMock.Object, factoryTypeMock.Object);

        return (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock);
    }

    private (Mock<IOperationFactory<Income>> factoryOperationMock,
            Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>> factoryTypeMock)
            Create_type_and_operations_factorys()
    
    {
        var factoryOperationMock = new Mock<IOperationFactory<Income>>();
        factoryOperationMock.Setup(f => f.Create(It.IsAny<DateTime>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Returns((DateTime dateOfAction, double amount, string type, string comment) => new Income(dateOfAction, amount, type, comment));
        var factoryTypeMock = new Mock<INameTypeOfOperationsFactory<NameTypeOfIncomes>>();
        factoryTypeMock.Setup(f => f.Create(It.IsAny<string>())).Returns((string name) => new NameTypeOfIncomes(name));
        return (factoryOperationMock, factoryTypeMock);
    }
}

