using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;

public class IncomesManipulatorMoqTests
{
    [Fact]
    public async Task InfoOfIncomes_IncomesExist_ShouldReturnList()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task InfoOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var result = await service.InfoOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no Incomes for now", result.Error);
    }

    [Fact]
    public async Task AddNewIncomes_IncomeUniqe_ShouldAdd()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Salary",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewIncome(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        repoIncomeMock.Verify(i => i.AddAsync(It.IsAny<Income>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddNewIncome_TypeDoesNotFound_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 300,
            Comment = "sdsdfsdfsdf",
        };

        var result = await service.AddNewIncome(dto);

        Assert.True(result.IsFailure);
        Assert.Equal("This type of Incomes was not found", result.Error);
        repoIncomeMock.Verify(i => i.AddAsync(It.IsAny<Income>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_IncomesIdExist_ShouldUpdate()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary"),
            new TypeOfIncomes("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Other",
            Amount = 450,
            Comment = "sdsdfsdfsdf",
        };

        var income = existIncomes.First(i => i.TypeOfIncomes == "Salary");

        var result = await service.Update(dto, income.Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_IncomeIdDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary"),
            new TypeOfIncomes("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var dto = new IncomeDto
        {
            TypeOfIncomes = "Other",
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
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary"),
            new TypeOfIncomes("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var income = existIncomes.First(i => i.TypeOfIncomes == "Salary");

        var result = await service.Delete(income.Id);

        Assert.True(result.IsSuccess);
        repoIncomeMock.Verify(i => i.Remove(It.IsAny<Income>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Delete_IncomeIdDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now,300,"Salary","sdfgdfgdgdfg")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary"),
            new TypeOfIncomes("Other")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock) = Create_Service_RepoMocks_UnitMock(existIncomes, existTypeOfIncomes);

        var result = await service.Delete(999999);

        Assert.True(result.IsFailure);
        Assert.Equal("There is no income with this (999999)ID", result.Error);
        repoIncomeMock.Verify(i => i.Remove(It.IsAny<Income>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private (IncomesManipulator service,
             Mock<IIncomeRepository> repoIncomeMock,
             Mock<ITypeOfIncomesRepository> repoOfTypeIncomes,
             Mock<IUnitOfWork> unitMock) Create_Service_RepoMocks_UnitMock(List<Income> existIncomes, List<TypeOfIncomes> existTypeOfIncomes)
    {
        var loggerMock = new Mock<ILogger<IncomesManipulator>>();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes.BuildMock().AsQueryable());

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesManipulator(unitMock.Object, loggerMock.Object);

        return (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock);
    }


}