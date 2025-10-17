using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;
using Microsoft.Extensions.Logging;

public class IncomesTypeManipulatorMoqTests
{
    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseDoesNotExist_ShouldLoad()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        await service.LoadTypeOfIncomes();

        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<TypeOfIncomes>()), Times.Exactly(2));
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        await service.LoadTypeOfIncomes();

        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<TypeOfIncomes>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no types of Incomes for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.GetInfoOfType(existTypeOfIncomes[0].Id);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.GetInfoOfType(9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesExist_ShouldShow()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.TotalSumOfIncomes();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.TotalSumOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsSuccess);
        Assert.Equal("Salary", result.Value.Name);
        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<TypeOfIncomes>()), Times.Once);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddType_TypeIsNotUniqe_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "Salary"
        };

        var result = await service.AddType(dto);

        Assert.True(result.IsFailure);
        Assert.Equal($"Name {dto.NameOfType} is already exists, try another name", result.Error);
        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<TypeOfIncomes>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Update_TypeExist_ShouldUpdate()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var dto = new TypeOfIncomesDto
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
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, 9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Delete_TypeExist_ShouldDelete()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.Delete(existTypeOfIncomes[0].Id);

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        repoOfTypeIncomesMock.Verify(t => t.Remove(It.IsAny<TypeOfIncomes>()), Times.Once);
        repoIncomeMock.Verify(i => i.RemoveRange(It.IsAny<List<Income>>()), Times.Once);
    }

    [Fact]
    public async Task Delete_TypeDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        };

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        };

        var (service, repoIncomeMock, repoOfTypeIncomesMock,unitMock) = CreateService(existIncomes,existTypeOfIncomes);

        var result = await service.Delete(9999);

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        repoOfTypeIncomesMock.Verify(t => t.Remove(It.IsAny<TypeOfIncomes>()), Times.Never);
        repoIncomeMock.Verify(i => i.RemoveRange(It.IsAny<List<Income>>()), Times.Never);
    }

    private (IncomesTypeManipulator service,
            Mock<IIncomeRepository> repoIncomeMock,
            Mock<ITypeOfIncomesRepository> repoOfTypeIncomesMock,
            Mock<IUnitOfWork> unitMock) CreateService(List<Income> existIncomes, List<TypeOfIncomes> existTypeOfIncomes)
    {
        var loggerMock = new Mock<ILogger<IncomesTypeManipulator>>();
        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes.BuildMock().AsQueryable());

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes.BuildMock().AsQueryable());

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object, loggerMock.Object);

        return (service, repoIncomeMock, repoOfTypeIncomesMock, unitMock);
    }
}

