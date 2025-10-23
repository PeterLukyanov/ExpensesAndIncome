using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;
using Factorys;

public class IncomesServiceMoqTests
{
    [Fact]
    public async Task InfoOfIncomes_IncomesExist_ShouldReturnList()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            factoryOperationMock.Object.Create(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var result = await service.InfoOfOperations();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var result = await service.InfoOfOperations();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes for now", result.Error);
    }

    [Fact]
    public async Task AddNewIncomes_IncomeUniqe_ShouldAdd()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();
        
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var dto = new OperationDto
        {
            Type = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        repoIncomeMock.Verify(i => i.AddAsync(It.IsAny<Income>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewIncome_TypeDoesNotFound_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewOperation(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"{dto.Type} type of incomes was not found", result.Error);
        repoIncomeMock.Verify(i => i.AddAsync(It.IsAny<Income>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_IncomesIdExist_ShouldUpdate()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary"),
            factoryTypeMock.Object.Create("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var income = existIncomes.First(i => i.Type == "Salary");

        var result = await service.Update(dto, income.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_IncomeIdDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary"),
            factoryTypeMock.Object.Create("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var dto = new OperationDto
        {
            Type = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no income with this (9999)ID", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_IncomeIdExist_ShouldDelete()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary"),
            factoryTypeMock.Object.Create("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var income = existIncomes.First(i => i.Type == "Salary");

        var result = await service.Delete(income.Id);

        Assert.True(result.IsSuccess);
        repoIncomeMock.Verify(i => i.Remove(It.IsAny<Income>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_IncomeIdDoesNotExist_ShouldFail()
    {
        var (factoryOperationMock, factoryTypeMock) = Create_type_and_operations_factorys();

        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<NameTypeOfIncomes>
        {
            factoryTypeMock.Object.Create("Salary"),
            factoryTypeMock.Object.Create("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes, factoryOperationMock);

        var result = await service.Delete(999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no income with this (999999)ID", result.Error);
        repoIncomeMock.Verify(i => i.Remove(It.IsAny<Income>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private (IncomesService service,
             Mock<IOperationRepository<Income>> repoIncomeMock,
             Mock<ITypeOfOperationRepository<NameTypeOfIncomes>> repoOfTypeIncomes,
             Mock<IUnitOfWork> unitMock)
              Create_Service_RepoMocks_UnitMock(List<Income> existIncomes,
                                                List<NameTypeOfIncomes> existTypeOfIncomes,
                                                Mock<IOperationFactory<Income>> factoryOperationMock)
    {
        var loggerMock = new Mock<ILogger<IncomesService>>();

        var repoIncomeMock = new Mock<IOperationRepository<Income>>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes.BuildMock().AsQueryable());

        var repoOfTypeIncomesMock = new Mock<ITypeOfOperationRepository<NameTypeOfIncomes>>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesService(unitMock.Object, loggerMock.Object, factoryOperationMock.Object);

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