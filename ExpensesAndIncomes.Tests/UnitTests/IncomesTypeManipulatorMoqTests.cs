using Models;
using MockQueryable;
using Repositorys;
using Moq;
using UoW;
using Services;
using Dtos;

public class IncomesTypeManipulatorMoqTests
{
    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseDoesNotExist_ShouldLoad()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        await service.LoadTypeOfIncomes();

        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<TypeOfIncomes>()), Times.Exactly(2));
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LoadTypeOfIncomes_DataBaseExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        await service.LoadTypeOfIncomes();

        repoOfTypeIncomesMock.Verify(t => t.AddAsync(It.IsAny<TypeOfIncomes>()), Times.Never);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldShow()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.InfoTypes();

        Assert.NotEmpty(result.Value);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task InfoTypes_TypesExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.InfoTypes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no types of Incomes for now", result.Error);
    }

    [Fact]
    public async Task GetInfoOfType_TypeExist_ShouldShow()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.GetInfoOfType("Salary");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetInfoOfType_TypeDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.GetInfoOfType("Other");

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesExist_ShouldShow()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.TotalSummOfIncomes();

        Assert.True(result.IsSuccess);
        Assert.Equal(300, result.Value);
    }

    [Fact]
    public async Task TotalSumOfIncomes_IncomesDoesNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.TotalSummOfIncomes();

        Assert.True(result.IsFailure);
        Assert.Equal("There are no incomes", result.Error);
    }

    [Fact]
    public async Task AddType_TypeIsUniqe_ShouldAdd()
    {
        var existIncomes = new List<Income>
        {

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {

        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

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

        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

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
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, "Salary");

        Assert.True(result.IsSuccess);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Update_TypeIsNotExist_ShouldFail()
    {
        var existIncomes = new List<Income>
        {
            new Income(DateTime.Now, 300, "Salary", "fsdfssadfsd")
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var dto = new TypeOfIncomesDto
        {
            NameOfType = "My Salary"
        };

        var result = await service.Update(dto, "Salar");

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
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.Delete("Salary");

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
        }.BuildMock().AsQueryable();

        var existTypeOfIncomes = new List<TypeOfIncomes>
        {
            new TypeOfIncomes("Salary")
        }.BuildMock().AsQueryable();

        var repoIncomeMock = new Mock<IIncomeRepository>();
        repoIncomeMock.Setup(i => i.GetAll()).Returns(existIncomes);

        var repoOfTypeIncomesMock = new Mock<ITypeOfIncomesRepository>();
        repoOfTypeIncomesMock.Setup(t => t.GetAll()).Returns(existTypeOfIncomes);

        var unitMock = new Mock<IUnitOfWork>();
        unitMock.Setup(t => t.incomeRepository).Returns(repoIncomeMock.Object);
        unitMock.Setup(t => t.typeOfIncomesRepository).Returns(repoOfTypeIncomesMock.Object);

        var service = new IncomesTypeManipulator(unitMock.Object);

        var result = await service.Delete("Salar");

        Assert.True(result.IsFailure);
        Assert.Equal("Such type of Incomes does not exist", result.Error);
        unitMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        repoOfTypeIncomesMock.Verify(t => t.Remove(It.IsAny<TypeOfIncomes>()), Times.Never);
        repoIncomeMock.Verify(i => i.RemoveRange(It.IsAny<List<Income>>()), Times.Never);
    }
}